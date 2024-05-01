﻿// ReSharper disable BuiltInTypeReferenceStyle
// ReSharper disable RedundantNameQualifier
// ReSharper disable ArrangeObjectCreationWhenTypeEvident
// ReSharper disable UnusedType.Global
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedMethodReturnValue.Local
// ReSharper disable ConvertToAutoProperty
// ReSharper disable UnusedMember.Global
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable InconsistentNaming

// AnyScalarDefaultSerializationClient

// <auto-generated/>
#nullable enable annotations
#nullable disable warnings

namespace Microsoft.Extensions.DependencyInjection
{
    // StrawberryShake.CodeGeneration.CSharp.Generators.DependencyInjectionGenerator
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public static partial class AnyScalarDefaultSerializationClientServiceCollectionExtensions
    {
        public static global::StrawberryShake.IClientBuilder<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.State.AnyScalarDefaultSerializationClientStoreAccessor> AddAnyScalarDefaultSerializationClient(this global::Microsoft.Extensions.DependencyInjection.IServiceCollection services, global::StrawberryShake.ExecutionStrategy strategy = global::StrawberryShake.ExecutionStrategy.NetworkOnly)
        {
            var serviceCollection = new global::Microsoft.Extensions.DependencyInjection.ServiceCollection();
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton(services, sp =>
            {
                ConfigureClientDefault(sp, serviceCollection, strategy);
                return new ClientServiceProvider(global::Microsoft.Extensions.DependencyInjection.ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(serviceCollection));
            });
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton(services, sp => new global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.State.AnyScalarDefaultSerializationClientStoreAccessor(global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.IOperationStore>(global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<ClientServiceProvider>(sp)), global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.IEntityStore>(global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<ClientServiceProvider>(sp)), global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.IEntityIdSerializer>(global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<ClientServiceProvider>(sp)), global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::System.Collections.Generic.IEnumerable<global::StrawberryShake.IOperationRequestFactory>>(global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<ClientServiceProvider>(sp)), global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::System.Collections.Generic.IEnumerable<global::StrawberryShake.IOperationResultDataFactory>>(global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<ClientServiceProvider>(sp))));
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton(services, sp => global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.GetJsonQuery>(global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<ClientServiceProvider>(sp)));
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton(services, sp => global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.AnyScalarDefaultSerializationClient>(global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<ClientServiceProvider>(sp)));
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton(services, sp => global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IAnyScalarDefaultSerializationClient>(global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<ClientServiceProvider>(sp)));
            return new global::StrawberryShake.ClientBuilder<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.State.AnyScalarDefaultSerializationClientStoreAccessor>("AnyScalarDefaultSerializationClient", services, serviceCollection);
        }

        private static global::Microsoft.Extensions.DependencyInjection.IServiceCollection ConfigureClientDefault(global::System.IServiceProvider parentServices, global::Microsoft.Extensions.DependencyInjection.ServiceCollection services, global::StrawberryShake.ExecutionStrategy strategy = global::StrawberryShake.ExecutionStrategy.NetworkOnly)
        {
            global::Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAddSingleton<global::StrawberryShake.IEntityStore, global::StrawberryShake.EntityStore>(services);
            global::Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAddSingleton<global::StrawberryShake.IOperationStore>(services, sp => new global::StrawberryShake.OperationStore(global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.IEntityStore>(sp)));
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Transport.Http.IHttpConnection>(services, sp =>
            {
                var clientFactory = global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::System.Net.Http.IHttpClientFactory>(parentServices);
                return new global::StrawberryShake.Transport.Http.HttpConnection(() => clientFactory.CreateClient("AnyScalarDefaultSerializationClient"));
            });
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.StringSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.BooleanSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.ByteSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.ShortSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.IntSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.LongSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.FloatSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.DecimalSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.UrlSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.UUIDSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.IdSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.DateTimeSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.DateSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.ByteArraySerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.TimeSpanSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializer, global::StrawberryShake.Serialization.JsonSerializer>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.Serialization.ISerializerResolver>(services, sp => new global::StrawberryShake.Serialization.SerializerResolver(global::System.Linq.Enumerable.Concat(global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::System.Collections.Generic.IEnumerable<global::StrawberryShake.Serialization.ISerializer>>(parentServices), global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::System.Collections.Generic.IEnumerable<global::StrawberryShake.Serialization.ISerializer>>(sp))));
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.IOperationResultDataFactory<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonResult>, global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.State.GetJsonResultFactory>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.IOperationResultDataFactory>(services, sp => global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.IOperationResultDataFactory<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonResult>>(sp));
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.IOperationRequestFactory>(services, sp => global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonQuery>(sp));
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.IOperationResultBuilder<global::System.Text.Json.JsonDocument, global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonResult>, global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.State.GetJsonBuilder>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.IOperationExecutor<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonResult>>(services, sp => new global::StrawberryShake.OperationExecutor<global::System.Text.Json.JsonDocument, global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonResult>(global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.Transport.Http.IHttpConnection>(sp), () => global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.IOperationResultBuilder<global::System.Text.Json.JsonDocument, global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonResult>>(sp), () => global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.IResultPatcher<global::System.Text.Json.JsonDocument>>(sp), global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.IOperationStore>(sp), strategy));
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.IResultPatcher<global::System.Text.Json.JsonDocument>, global::StrawberryShake.Json.JsonResultPatcher>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.GetJsonQuery>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonQuery>(services, sp => global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.GetJsonQuery>(sp));
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.IEntityIdSerializer, global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.State.AnyScalarDefaultSerializationClientEntityIdFactory>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.AnyScalarDefaultSerializationClient>(services);
            global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IAnyScalarDefaultSerializationClient>(services, sp => global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.AnyScalarDefaultSerializationClient>(sp));
            return services;
        }

        private sealed class ClientServiceProvider : System.IServiceProvider, System.IDisposable
        {
            private readonly System.IServiceProvider _provider;
            public ClientServiceProvider(System.IServiceProvider provider)
            {
                _provider = provider;
            }

            public object? GetService(System.Type serviceType)
            {
                return _provider.GetService(serviceType);
            }

            public void Dispose()
            {
                if (_provider is System.IDisposable d)
                {
                    d.Dispose();
                }
            }
        }
    }
}

namespace StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization
{
    // StrawberryShake.CodeGeneration.CSharp.Generators.ResultTypeGenerator
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public partial class GetJsonResult : global::System.IEquatable<GetJsonResult>, IGetJsonResult
    {
        public GetJsonResult(global::System.Text.Json.JsonElement json)
        {
            Json = json;
        }

        public global::System.Text.Json.JsonElement Json { get; }

        public virtual global::System.Boolean Equals(GetJsonResult? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != GetType())
            {
                return false;
            }

            return (global::System.Object.Equals(Json, other.Json));
        }

        public override global::System.Boolean Equals(global::System.Object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((GetJsonResult)obj);
        }

        public override global::System.Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 5;
                hash ^= 397 * Json.GetHashCode();
                return hash;
            }
        }
    }

    // StrawberryShake.CodeGeneration.CSharp.Generators.ResultInterfaceGenerator
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public partial interface IGetJsonResult
    {
        public global::System.Text.Json.JsonElement Json { get; }
    }

    // StrawberryShake.CodeGeneration.CSharp.Generators.OperationDocumentGenerator
    /// <summary>
    /// Represents the operation service of the GetJson GraphQL operation
    /// <code>
    /// query GetJson {
    ///   json
    /// }
    /// </code>
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public partial class GetJsonQueryDocument : global::StrawberryShake.IDocument
    {
        private GetJsonQueryDocument()
        {
        }

        public static GetJsonQueryDocument Instance { get; } = new GetJsonQueryDocument();
        public global::StrawberryShake.OperationKind Kind => global::StrawberryShake.OperationKind.Query;
        public global::System.ReadOnlySpan<global::System.Byte> Body => new global::System.Byte[]{0x71, 0x75, 0x65, 0x72, 0x79, 0x20, 0x47, 0x65, 0x74, 0x4a, 0x73, 0x6f, 0x6e, 0x20, 0x7b, 0x20, 0x6a, 0x73, 0x6f, 0x6e, 0x20, 0x7d};
        public global::StrawberryShake.DocumentHash Hash { get; } = new global::StrawberryShake.DocumentHash("sha1Hash", "90a00e07ca153decc5937eb356401940e7c6b66a");
        public override global::System.String ToString()
        {
#if NETCOREAPP3_1_OR_GREATER
        return global::System.Text.Encoding.UTF8.GetString(Body);
#else
            return global::System.Text.Encoding.UTF8.GetString(Body.ToArray());
#endif
        }
    }

    // StrawberryShake.CodeGeneration.CSharp.Generators.OperationServiceGenerator
    /// <summary>
    /// Represents the operation service of the GetJson GraphQL operation
    /// <code>
    /// query GetJson {
    ///   json
    /// }
    /// </code>
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public partial class GetJsonQuery : global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonQuery
    {
        private readonly global::StrawberryShake.IOperationExecutor<IGetJsonResult> _operationExecutor;
        public GetJsonQuery(global::StrawberryShake.IOperationExecutor<IGetJsonResult> operationExecutor)
        {
            _operationExecutor = operationExecutor ?? throw new global::System.ArgumentNullException(nameof(operationExecutor));
        }

        global::System.Type global::StrawberryShake.IOperationRequestFactory.ResultType => typeof(IGetJsonResult);
        public async global::System.Threading.Tasks.Task<global::StrawberryShake.IOperationResult<IGetJsonResult>> ExecuteAsync(global::System.Threading.CancellationToken cancellationToken = default)
        {
            var request = CreateRequest();
            return await _operationExecutor.ExecuteAsync(request, cancellationToken).ConfigureAwait(false);
        }

        public global::System.IObservable<global::StrawberryShake.IOperationResult<IGetJsonResult>> Watch(global::StrawberryShake.ExecutionStrategy? strategy = null)
        {
            var request = CreateRequest();
            return _operationExecutor.Watch(request, strategy);
        }

        private global::StrawberryShake.OperationRequest CreateRequest()
        {
            return CreateRequest(null);
        }

        private global::StrawberryShake.OperationRequest CreateRequest(global::System.Collections.Generic.IReadOnlyDictionary<global::System.String, global::System.Object?>? variables)
        {
            return new global::StrawberryShake.OperationRequest(id: GetJsonQueryDocument.Instance.Hash.Value, name: "GetJson", document: GetJsonQueryDocument.Instance, strategy: global::StrawberryShake.RequestStrategy.Default);
        }

        global::StrawberryShake.OperationRequest global::StrawberryShake.IOperationRequestFactory.Create(global::System.Collections.Generic.IReadOnlyDictionary<global::System.String, global::System.Object?>? variables)
        {
            return CreateRequest();
        }
    }

    // StrawberryShake.CodeGeneration.CSharp.Generators.OperationServiceInterfaceGenerator
    /// <summary>
    /// Represents the operation service of the GetJson GraphQL operation
    /// <code>
    /// query GetJson {
    ///   json
    /// }
    /// </code>
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public partial interface IGetJsonQuery : global::StrawberryShake.IOperationRequestFactory
    {
        global::System.Threading.Tasks.Task<global::StrawberryShake.IOperationResult<IGetJsonResult>> ExecuteAsync(global::System.Threading.CancellationToken cancellationToken = default);
        global::System.IObservable<global::StrawberryShake.IOperationResult<IGetJsonResult>> Watch(global::StrawberryShake.ExecutionStrategy? strategy = null);
    }

    // StrawberryShake.CodeGeneration.CSharp.Generators.ClientGenerator
    /// <summary>
    /// Represents the AnyScalarDefaultSerializationClient GraphQL client
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public partial class AnyScalarDefaultSerializationClient : global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IAnyScalarDefaultSerializationClient
    {
        private readonly global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonQuery _getJson;
        public AnyScalarDefaultSerializationClient(global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonQuery getJson)
        {
            _getJson = getJson ?? throw new global::System.ArgumentNullException(nameof(getJson));
        }

        public static global::System.String ClientName => "AnyScalarDefaultSerializationClient";
        public global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonQuery GetJson => _getJson;
    }

    // StrawberryShake.CodeGeneration.CSharp.Generators.ClientInterfaceGenerator
    /// <summary>
    /// Represents the AnyScalarDefaultSerializationClient GraphQL client
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public partial interface IAnyScalarDefaultSerializationClient
    {
        global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonQuery GetJson { get; }
    }
}

namespace StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.State
{
    // StrawberryShake.CodeGeneration.CSharp.Generators.ResultDataFactoryGenerator
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public partial class GetJsonResultFactory : global::StrawberryShake.IOperationResultDataFactory<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.GetJsonResult>
    {
        private readonly global::StrawberryShake.IEntityStore _entityStore;
        public GetJsonResultFactory(global::StrawberryShake.IEntityStore entityStore)
        {
            _entityStore = entityStore ?? throw new global::System.ArgumentNullException(nameof(entityStore));
        }

        global::System.Type global::StrawberryShake.IOperationResultDataFactory.ResultType => typeof(global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonResult);
        public GetJsonResult Create(global::StrawberryShake.IOperationResultDataInfo dataInfo, global::StrawberryShake.IEntityStoreSnapshot? snapshot = null)
        {
            if (snapshot is null)
            {
                snapshot = _entityStore.CurrentSnapshot;
            }

            if (dataInfo is GetJsonResultInfo info)
            {
                return new GetJsonResult(info.Json);
            }

            throw new global::System.ArgumentException("GetJsonResultInfo expected.");
        }

        global::System.Object global::StrawberryShake.IOperationResultDataFactory.Create(global::StrawberryShake.IOperationResultDataInfo dataInfo, global::StrawberryShake.IEntityStoreSnapshot? snapshot)
        {
            return Create(dataInfo, snapshot);
        }
    }

    // StrawberryShake.CodeGeneration.CSharp.Generators.ResultInfoGenerator
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public partial class GetJsonResultInfo : global::StrawberryShake.IOperationResultDataInfo
    {
        private readonly global::System.Collections.Generic.IReadOnlyCollection<global::StrawberryShake.EntityId> _entityIds;
        private readonly global::System.UInt64 _version;
        public GetJsonResultInfo(global::System.Text.Json.JsonElement json, global::System.Collections.Generic.IReadOnlyCollection<global::StrawberryShake.EntityId> entityIds, global::System.UInt64 version)
        {
            Json = json;
            _entityIds = entityIds ?? throw new global::System.ArgumentNullException(nameof(entityIds));
            _version = version;
        }

        public global::System.Text.Json.JsonElement Json { get; }

        public global::System.Collections.Generic.IReadOnlyCollection<global::StrawberryShake.EntityId> EntityIds => _entityIds;
        public global::System.UInt64 Version => _version;
        public global::StrawberryShake.IOperationResultDataInfo WithVersion(global::System.UInt64 version)
        {
            return new GetJsonResultInfo(Json, _entityIds, version);
        }
    }

    // StrawberryShake.CodeGeneration.CSharp.Generators.JsonResultBuilderGenerator
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public partial class GetJsonBuilder : global::StrawberryShake.OperationResultBuilder<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonResult>
    {
        private readonly global::StrawberryShake.IEntityStore _entityStore;
        private readonly global::StrawberryShake.IEntityIdSerializer _idSerializer;
        private readonly global::StrawberryShake.Serialization.ILeafValueParser<global::System.Text.Json.JsonElement, global::System.Text.Json.JsonElement> _anyParser;
        public GetJsonBuilder(global::StrawberryShake.IEntityStore entityStore, global::StrawberryShake.IEntityIdSerializer idSerializer, global::StrawberryShake.IOperationResultDataFactory<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonResult> resultDataFactory, global::StrawberryShake.Serialization.ISerializerResolver serializerResolver)
        {
            _entityStore = entityStore ?? throw new global::System.ArgumentNullException(nameof(entityStore));
            _idSerializer = idSerializer ?? throw new global::System.ArgumentNullException(nameof(idSerializer));
            ResultDataFactory = resultDataFactory ?? throw new global::System.ArgumentNullException(nameof(resultDataFactory));
            _anyParser = serializerResolver.GetLeafValueParser<global::System.Text.Json.JsonElement, global::System.Text.Json.JsonElement>("Any") ?? throw new global::System.ArgumentException("No serializer for type `Any` found.");
        }

        protected override global::StrawberryShake.IOperationResultDataFactory<global::StrawberryShake.CodeGeneration.CSharp.Integration.AnyScalarDefaultSerialization.IGetJsonResult> ResultDataFactory { get; }

        protected override global::StrawberryShake.IOperationResultDataInfo BuildData(global::System.Text.Json.JsonElement obj)
        {
            var entityIds = new global::System.Collections.Generic.HashSet<global::StrawberryShake.EntityId>();
            global::StrawberryShake.IEntityStoreSnapshot snapshot = default !;
            _entityStore.Update(session =>
            {
                snapshot = session.CurrentSnapshot;
            });
            return new GetJsonResultInfo(Deserialize_NonNullableJsonElement(global::StrawberryShake.Json.JsonElementExtensions.GetPropertyOrNull(obj, "json")), entityIds, snapshot.Version);
        }

        private global::System.Text.Json.JsonElement Deserialize_NonNullableJsonElement(global::System.Text.Json.JsonElement? obj)
        {
            if (!obj.HasValue)
            {
                throw new global::System.ArgumentNullException();
            }

            if (obj.Value.ValueKind == global::System.Text.Json.JsonValueKind.Null)
            {
                throw new global::System.ArgumentNullException();
            }

            return _anyParser.Parse(obj.Value!);
        }
    }

    // StrawberryShake.CodeGeneration.CSharp.Generators.EntityIdFactoryGenerator
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public partial class AnyScalarDefaultSerializationClientEntityIdFactory : global::StrawberryShake.IEntityIdSerializer
    {
        private static readonly global::System.Text.Json.JsonWriterOptions _options = new global::System.Text.Json.JsonWriterOptions()
        {Indented = false};
        public global::StrawberryShake.EntityId Parse(global::System.Text.Json.JsonElement obj)
        {
            var __typename = obj.GetProperty("__typename").GetString()!;
            return __typename switch
            {
                _ => throw new global::System.NotSupportedException()};
        }

        public global::System.String Format(global::StrawberryShake.EntityId entityId)
        {
            return entityId.Name switch
            {
                _ => throw new global::System.NotSupportedException()};
        }
    }

    // StrawberryShake.CodeGeneration.CSharp.Generators.StoreAccessorGenerator
    [global::System.CodeDom.Compiler.GeneratedCode("StrawberryShake", "11.0.0")]
    public partial class AnyScalarDefaultSerializationClientStoreAccessor : global::StrawberryShake.StoreAccessor
    {
        public AnyScalarDefaultSerializationClientStoreAccessor(global::StrawberryShake.IOperationStore operationStore, global::StrawberryShake.IEntityStore entityStore, global::StrawberryShake.IEntityIdSerializer entityIdSerializer, global::System.Collections.Generic.IEnumerable<global::StrawberryShake.IOperationRequestFactory> requestFactories, global::System.Collections.Generic.IEnumerable<global::StrawberryShake.IOperationResultDataFactory> resultDataFactories) : base(operationStore, entityStore, entityIdSerializer, requestFactories, resultDataFactories)
        {
        }
    }
}


