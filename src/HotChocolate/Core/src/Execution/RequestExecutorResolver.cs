using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
#if NET6_0_OR_GREATER
using System.Reflection.Metadata;
#endif
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Configuration;
#if NET6_0_OR_GREATER
using HotChocolate.Execution;
#endif
using HotChocolate.Execution.Configuration;
using HotChocolate.Execution.Errors;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution.Internal;
using HotChocolate.Execution.Options;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;
using HotChocolate.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using static HotChocolate.Execution.ThrowHelper;

#if NET6_0_OR_GREATER
[assembly: MetadataUpdateHandler(typeof(RequestExecutorResolver.ApplicationUpdateHandler))]
#endif

namespace HotChocolate.Execution;

internal sealed partial class RequestExecutorResolver
    : IRequestExecutorResolver
    , IInternalRequestExecutorResolver
    , IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly ConcurrentDictionary<string, RegisteredExecutor> _executors = new();
    private readonly IRequestExecutorOptionsMonitor _optionsMonitor;
    private readonly IServiceProvider _applicationServices;
    private readonly EventObservable _events = new();
    private ulong _version;
    private bool _disposed;

    [Obsolete("Use the events property instead.")]
    public event EventHandler<RequestExecutorEvictedEventArgs>? RequestExecutorEvicted;

    public RequestExecutorResolver(
        IRequestExecutorOptionsMonitor optionsMonitor,
        IServiceProvider serviceProvider)
    {
        _optionsMonitor = optionsMonitor ??
            throw new ArgumentNullException(nameof(optionsMonitor));
        _applicationServices = serviceProvider ??
            throw new ArgumentNullException(nameof(serviceProvider));
        _optionsMonitor.OnChange(EvictRequestExecutor);

#if NET6_0_OR_GREATER
        // we register the schema eviction for application updates when hot reload is used.
        // Whenever a hot reload update is triggered we will evict all executors.
        ApplicationUpdateHandler.RegisterForApplicationUpdate(() => EvictAllRequestExecutors());
#endif
    }

    public IObservable<RequestExecutorEvent> Events => _events;

    public async ValueTask<IRequestExecutor> GetRequestExecutorAsync(
        string? schemaName = default,
        CancellationToken cancellationToken = default)
    {
        schemaName ??= Schema.DefaultName;

        if (!_executors.TryGetValue(schemaName, out var re))
        {
            await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                return await GetRequestExecutorNoLockAsync(schemaName, cancellationToken)
                    .ConfigureAwait(false);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        return re.Executor;
    }

    public async ValueTask<IRequestExecutor> GetRequestExecutorNoLockAsync(
        string? schemaName = default,
        CancellationToken cancellationToken = default)
    {
        schemaName ??= Schema.DefaultName;

        if (!_executors.TryGetValue(schemaName, out var registeredExecutor))
        {
            var setup =
                await _optionsMonitor.GetAsync(schemaName, cancellationToken)
                    .ConfigureAwait(false);

            var context = new ConfigurationContext(
                schemaName,
                setup.SchemaBuilder ?? new SchemaBuilder(),
                _applicationServices);

            var schemaServices =
                await CreateSchemaServicesAsync(context, setup, cancellationToken)
                    .ConfigureAwait(false);

            registeredExecutor = new RegisteredExecutor(
                schemaServices.GetRequiredService<IRequestExecutor>(),
                schemaServices,
                schemaServices.GetRequiredService<IExecutionDiagnosticEvents>(),
                setup,
                schemaServices.GetRequiredService<TypeModuleChangeMonitor>());

            var executor = registeredExecutor.Executor;

            await OnRequestExecutorCreatedAsync(context, executor, setup, cancellationToken)
                .ConfigureAwait(false);

            registeredExecutor.DiagnosticEvents.ExecutorCreated(
                schemaName,
                registeredExecutor.Executor);
            _executors.TryAdd(schemaName, registeredExecutor);

            _events.RaiseEvent(
                new RequestExecutorEvent(
                    RequestExecutorEventType.Created,
                    schemaName,
                    registeredExecutor.Executor));
        }

        return registeredExecutor.Executor;
    }

    public void EvictRequestExecutor(string? schemaName = default)
    {
        schemaName ??= Schema.DefaultName;

        if (_executors.TryRemove(schemaName, out var re))
        {
            re.DiagnosticEvents.ExecutorEvicted(schemaName, re.Executor);

            try
            {
                RequestExecutorEvicted?.Invoke(
                    this,
                    new RequestExecutorEvictedEventArgs(schemaName, re.Executor));
                _events.RaiseEvent(
                    new RequestExecutorEvent(
                        RequestExecutorEventType.Evicted,
                        schemaName,
                        re.Executor));
            }
            finally
            {
                BeginRunEvictionEvents(re);
            }
        }
    }

#if NET6_0_OR_GREATER
    private void EvictAllRequestExecutors()
    {
        foreach (var key in _executors.Keys)
        {
            if (_executors.TryRemove(key, out var re))
            {
                re.DiagnosticEvents.ExecutorEvicted(key, re.Executor);

                try
                {
                    RequestExecutorEvicted?.Invoke(
                        this,
                        new RequestExecutorEvictedEventArgs(key, re.Executor));
                    _events.RaiseEvent(
                        new RequestExecutorEvent(
                            RequestExecutorEventType.Evicted,
                            key,
                            re.Executor));
                }
                finally
                {
                    BeginRunEvictionEvents(re);
                }
            }
        }
    }
#endif

    private static void BeginRunEvictionEvents(RegisteredExecutor registeredExecutor)
        => Task.Factory.StartNew(
            async () =>
            {
                try
                {
                    await OnRequestExecutorEvictedAsync(registeredExecutor);
                }
                finally
                {
                    // we will give the request executor some grace period to finish all request
                    // in the pipeline
                    await Task.Delay(TimeSpan.FromMinutes(5));
                    registeredExecutor.Dispose();
                }
            },
            default,
            TaskCreationOptions.DenyChildAttach,
            TaskScheduler.Default);

    private async Task<IServiceProvider> CreateSchemaServicesAsync(
        ConfigurationContext context,
        RequestExecutorSetup setup,
        CancellationToken cancellationToken)
    {
        ulong version;

        unchecked
        {
            version = ++_version;
        }

        var serviceCollection = new ServiceCollection();
        var typeModuleChangeMonitor = new TypeModuleChangeMonitor(this, context.SchemaName);
        var lazy = new SchemaBuilder.LazySchema();

        var executorOptions =
            await OnConfigureRequestExecutorOptionsAsync(context, setup, cancellationToken)
                .ConfigureAwait(false);

        // if there are any type modules we will register them with the
        // type module change monitor.
        // The module will track if type modules signal changes to the schema and
        // start a schema eviction.
        foreach (var typeModule in setup.TypeModules)
        {
            typeModuleChangeMonitor.Register(typeModule);
        }

        // we allow newer type modules to apply configurations.
        await typeModuleChangeMonitor.ConfigureAsync(context, cancellationToken)
            .ConfigureAwait(false);

        serviceCollection.AddSingleton<IApplicationServiceProvider>(
            _ => new DefaultApplicationServiceProvider(_applicationServices));

        serviceCollection.AddSingleton(_ => lazy.Schema);
        serviceCollection.AddSingleton(typeModuleChangeMonitor);
        serviceCollection.AddSingleton(executorOptions);
        serviceCollection.AddSingleton<IRequestExecutorOptionsAccessor>(
            s => s.GetRequiredService<RequestExecutorOptions>());
        serviceCollection.AddSingleton<IErrorHandlerOptionsAccessor>(
            s => s.GetRequiredService<RequestExecutorOptions>());
        serviceCollection.AddSingleton<IRequestTimeoutOptionsAccessor>(
            s => s.GetRequiredService<RequestExecutorOptions>());
        serviceCollection.AddSingleton<IPersistedQueryOptionsAccessor>(
            s => s.GetRequiredService<RequestExecutorOptions>());

        serviceCollection.AddSingleton<IErrorHandler, DefaultErrorHandler>();

        serviceCollection.TryAddDiagnosticEvents();
        serviceCollection.TryAddOperationExecutors();
        serviceCollection.TryAddTimespanProvider();

        // register global error filters
        foreach (var errorFilter in _applicationServices.GetServices<IErrorFilter>())
        {
            serviceCollection.AddSingleton(errorFilter);
        }

        // register global diagnostic listener
        foreach (var diagnosticEventListener in
            _applicationServices.GetServices<IExecutionDiagnosticEventListener>())
        {
            serviceCollection.AddSingleton(diagnosticEventListener);
        }

        serviceCollection.AddSingleton(
            sp => CreatePipeline(
                context.SchemaName,
                setup.DefaultPipelineFactory,
                setup.Pipeline,
                sp,
                sp.GetRequiredService<IRequestExecutorOptionsAccessor>()));

        serviceCollection.TryAddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

        serviceCollection.TryAddSingleton(
            sp =>
            {
                var provider = sp.GetRequiredService<ObjectPoolProvider>();
                var policy = new RequestContextPooledObjectPolicy(
                    sp.GetRequiredService<ISchema>(),
                    sp.GetRequiredService<IErrorHandler>(),
                    sp.GetRequiredService<IExecutionDiagnosticEvents>(),
                    version);
                return provider.Create(policy);
            });

        serviceCollection.AddSingleton<IRequestExecutor>(
            sp => new RequestExecutor(
                sp.GetRequiredService<ISchema>(),
                _applicationServices,
                sp,
                sp.GetRequiredService<RequestDelegate>(),
                sp.GetRequiredService<ObjectPool<RequestContext>>(),
                sp.GetApplicationService<DefaultRequestContextAccessor>(),
                version));

        OnConfigureSchemaServices(context, serviceCollection, setup);

        var schemaServices = serviceCollection.BuildServiceProvider();

        lazy.Schema =
            await CreateSchemaAsync(
                    context,
                    setup,
                    executorOptions,
                    new CombinedServiceProvider(schemaServices, _applicationServices),
                    typeModuleChangeMonitor,
                    cancellationToken)
                .ConfigureAwait(false);

        return schemaServices;
    }

    private static async ValueTask<ISchema> CreateSchemaAsync(
        ConfigurationContext context,
        RequestExecutorSetup setup,
        RequestExecutorOptions executorOptions,
        IServiceProvider schemaServices,
        TypeModuleChangeMonitor typeModuleChangeMonitor,
        CancellationToken cancellationToken)
    {
        if (setup.Schema is not null)
        {
            AssertSchemaNameValid(setup.Schema, context.SchemaName);
            return setup.Schema;
        }

        context
            .SchemaBuilder
            .AddServices(schemaServices)
            .SetContextData(typeof(RequestExecutorOptions).FullName!, executorOptions);

        var descriptorContext = context.SchemaBuilder.CreateContext();

        await foreach (var member in
            typeModuleChangeMonitor.CreateTypesAsync(descriptorContext)
                .WithCancellation(cancellationToken)
                .ConfigureAwait(false))
        {
            switch (member)
            {
                case INamedType namedType:
                    context.SchemaBuilder.AddType(namedType);
                    break;

                case INamedTypeExtension typeExtension:
                    context.SchemaBuilder.AddType(typeExtension);
                    break;
            }
        }

        await OnConfigureSchemaBuilderAsync(context, schemaServices, setup, cancellationToken);

        context.SchemaBuilder.TryAddTypeInterceptor(
            new SetSchemaNameInterceptor(context.SchemaName));

        var schema = context.SchemaBuilder.Create(descriptorContext);
        AssertSchemaNameValid(schema, context.SchemaName);
        return schema;
    }

    private static void AssertSchemaNameValid(ISchema schema, string expectedSchemaName)
    {
        if (!schema.Name.EqualsOrdinal(expectedSchemaName))
        {
            throw RequestExecutorResolver_SchemaNameDoesNotMatch(
                expectedSchemaName,
                schema.Name);
        }
    }

    private RequestDelegate CreatePipeline(
        string schemaName,
        Action<IList<RequestCoreMiddleware>>? defaultPipelineFactory,
        IList<RequestCoreMiddleware> pipeline,
        IServiceProvider schemaServices,
        IRequestExecutorOptionsAccessor options)
    {
        if (pipeline.Count == 0)
        {
            defaultPipelineFactory ??= RequestExecutorBuilderExtensions.AddDefaultPipeline;
            defaultPipelineFactory(pipeline);
        }

        var factoryContext = new RequestCoreMiddlewareContext(
            schemaName,
            _applicationServices,
            schemaServices,
            options);

        RequestDelegate next = _ => default;

        for (var i = pipeline.Count - 1; i >= 0; i--)
        {
            next = pipeline[i](factoryContext, next);
        }

        return next;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _events.Dispose();
            _executors.Clear();
            _semaphore.Dispose();
            _disposed = true;
        }
    }

    private sealed class RegisteredExecutor : IDisposable
    {
        private bool _disposed;

        public RegisteredExecutor(
            IRequestExecutor executor,
            IServiceProvider services,
            IExecutionDiagnosticEvents diagnosticEvents,
            RequestExecutorSetup setup,
            TypeModuleChangeMonitor typeModuleChangeMonitor)
        {
            Executor = executor;
            Services = services;
            DiagnosticEvents = diagnosticEvents;
            Setup = setup;
            TypeModuleChangeMonitor = typeModuleChangeMonitor;
        }

        public IRequestExecutor Executor { get; }

        public IServiceProvider Services { get; }

        public IExecutionDiagnosticEvents DiagnosticEvents { get; }

        public RequestExecutorSetup Setup { get; }

        public TypeModuleChangeMonitor TypeModuleChangeMonitor { get; }

        public void Dispose()
        {
            if (_disposed)
            {
                if (Services is IDisposable d)
                {
                    d.Dispose();
                }

                TypeModuleChangeMonitor.Dispose();
                _disposed = true;
            }
        }
    }

    private sealed class SetSchemaNameInterceptor : TypeInterceptor
    {
        private readonly string _schemaName;

        public SetSchemaNameInterceptor(string schemaName)
        {
            _schemaName = schemaName;
        }

        public override void OnBeforeCompleteName(
            ITypeCompletionContext completionContext,
            DefinitionBase definition)
        {
            if (completionContext.IsSchema)
            {
                definition.Name = _schemaName;
            }
        }
    }

    private sealed class TypeModuleChangeMonitor : IDisposable
    {
        private readonly List<ITypeModule> _typeModules = [];
        private readonly RequestExecutorResolver _resolver;
        private bool _disposed;

        public TypeModuleChangeMonitor(RequestExecutorResolver resolver, string schemaName)
        {
            _resolver = resolver;
            SchemaName = schemaName;
        }

        public string SchemaName { get; }

        public void Register(ITypeModule typeModule)
        {
            typeModule.TypesChanged += EvictRequestExecutor;
            _typeModules.Add(typeModule);
        }

        internal async ValueTask ConfigureAsync(
            ConfigurationContext context,
            CancellationToken cancellationToken)
        {
            foreach (var item in _typeModules)
            {
                if (item is TypeModule typeModule)
                {
                    await typeModule.ConfigureAsync(context, cancellationToken)
                        .ConfigureAwait(false);
                }
            }
        }

        public IAsyncEnumerable<ITypeSystemMember> CreateTypesAsync(IDescriptorContext context)
            => new TypeModuleEnumerable(_typeModules, context);

        private void EvictRequestExecutor(object? sender, EventArgs args)
            => _resolver.EvictRequestExecutor(SchemaName);

        public void Dispose()
        {
            if (!_disposed)
            {
                foreach (var typeModule in _typeModules)
                {
                    typeModule.TypesChanged -= EvictRequestExecutor;
                }

                _typeModules.Clear();
                _disposed = true;
            }
        }

        private sealed class TypeModuleEnumerable : IAsyncEnumerable<ITypeSystemMember>
        {
            private readonly List<ITypeModule> _typeModules;
            private readonly IDescriptorContext _context;

            public TypeModuleEnumerable(
                List<ITypeModule> typeModules,
                IDescriptorContext context)
            {
                _typeModules = typeModules;
                _context = context;
            }

            public async IAsyncEnumerator<ITypeSystemMember> GetAsyncEnumerator(
                CancellationToken cancellationToken = default)
            {
                foreach (var typeModule in _typeModules)
                {
                    var types =
                        await typeModule.CreateTypesAsync(_context, cancellationToken)
                            .ConfigureAwait(false);

                    foreach (var type in types)
                    {
                        yield return type;
                    }
                }
            }
        }
    }

    private sealed class RequestContextPooledObjectPolicy : PooledObjectPolicy<RequestContext>
    {
        private readonly ISchema _schema;
        private readonly ulong _executorVersion;
        private readonly IErrorHandler _errorHandler;
        private readonly IExecutionDiagnosticEvents _diagnosticEvents;

        public RequestContextPooledObjectPolicy(
            ISchema schema,
            IErrorHandler errorHandler,
            IExecutionDiagnosticEvents diagnosticEvents,
            ulong executorVersion)
        {
            _schema = schema ??
                throw new ArgumentNullException(nameof(schema));
            _errorHandler = errorHandler ??
                throw new ArgumentNullException(nameof(errorHandler));
            _diagnosticEvents = diagnosticEvents ??
                throw new ArgumentNullException(nameof(diagnosticEvents));
            _executorVersion = executorVersion;
        }


        public override RequestContext Create()
            => new(_schema, _executorVersion, _errorHandler, _diagnosticEvents);

        public override bool Return(RequestContext obj)
        {
            obj.Reset();
            return true;
        }
    }

    private sealed class EventObservable : IObservable<RequestExecutorEvent>, IDisposable
    {
        private readonly object _sync = new();
        private readonly List<Subscription> _subscriptions = [];
        private bool _disposed;

        public IDisposable Subscribe(IObserver<RequestExecutorEvent> observer)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(EventObservable));
            }

            if (observer is null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            var subscription = new Subscription(this, observer);

            lock (_sync)
            {
                _subscriptions.Add(subscription);
            }

            return subscription;
        }

        public void RaiseEvent(RequestExecutorEvent eventMessage)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(EventObservable));
            }

            lock (_sync)
            {
                foreach (var subscription in _subscriptions)
                {
                    subscription.Observer.OnNext(eventMessage);
                }
            }
        }

        private void Unsubscribe(Subscription subscription)
        {
            lock (_sync)
            {
                _subscriptions.Remove(subscription);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                lock (_sync)
                {
                    foreach (var subscription in _subscriptions)
                    {
                        subscription.Observer.OnCompleted();
                    }

                    _subscriptions.Clear();
                }

                _disposed = true;
            }
        }

        private sealed class Subscription : IDisposable
        {
            private readonly EventObservable _parent;
            private bool _disposed;

            public Subscription(EventObservable parent, IObserver<RequestExecutorEvent> observer)
            {
                _parent = parent;
                Observer = observer;
            }

            public IObserver<RequestExecutorEvent> Observer { get; }

            public void Dispose()
            {
                if (!_disposed)
                {
                    _parent.Unsubscribe(this);
                    _disposed = true;
                }
            }
        }
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// A helper calls that receives hot reload update events from the runtime and triggers
    /// reload of registered components.
    /// </summary>
    internal static class ApplicationUpdateHandler
    {
        private static readonly List<Action> _actions = [];

        public static void RegisterForApplicationUpdate(Action action)
            => _actions.Add(action);

        public static void UpdateApplication(Type[]? updatedTypes)
        {
            foreach (var action in _actions)
            {
                action();
            }
        }
    }
#endif
}
