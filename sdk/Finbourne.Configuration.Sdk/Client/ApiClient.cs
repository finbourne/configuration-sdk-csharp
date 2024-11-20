/*
 * FINBOURNE ConfigurationService API
 *
 * Contact: info@finbourne.com
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Finbourne.Sdk.Core.RestSharp;
using Finbourne.Sdk.Core.RestSharp.Serializers;
using Polly;
using Finbourne.Configuration.Sdk.Client.Auth;
using Finbourne.Configuration.Sdk.Extensions;


namespace Finbourne.Configuration.Sdk.Client
{
    /// <summary>
    /// Allows Finbourne.Sdk.Core.RestSharp to Serialize/Deserialize JSON using our custom logic, but only when ContentType is JSON.
    /// </summary>
    internal class CustomJsonCodec : IRestSerializer, ISerializer, IDeserializer
    {
        private readonly IReadableConfiguration _configuration;
        private static readonly ContentType _contentType = ContentType.Json;
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            // OpenAPI generated types generally hide default constructors.
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    OverrideSpecifiedNames = false
                }
            },
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        public CustomJsonCodec(IReadableConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CustomJsonCodec(JsonSerializerSettings serializerSettings, IReadableConfiguration configuration)
        {
            _serializerSettings = serializerSettings;
            _configuration = configuration;
        }

        /// <summary>
        /// Serialize the object into a JSON string.
        /// </summary>
        /// <param name="obj">Object to be serialized.</param>
        /// <returns>A JSON string.</returns>
        public string Serialize(object obj)
        {
            if (obj != null && obj is Finbourne.Configuration.Sdk.Model.AbstractOpenAPISchema)
            {
                // the object to be serialized is an oneOf/anyOf schema
                return ((Finbourne.Configuration.Sdk.Model.AbstractOpenAPISchema)obj).ToJson();
            }
            else
            {
                return JsonConvert.SerializeObject(obj, _serializerSettings);
            }
        }

        public string Serialize(Parameter bodyParameter) => Serialize(bodyParameter.Value);

        public T Deserialize<T>(RestResponse response)
        {
            var result = (T)Deserialize(response, typeof(T));
            return result;
        }

        /// <summary>
        /// Deserialize the JSON string into a proper object.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="type">Object type.</param>
        /// <returns>Object representation of the JSON string.</returns>
        internal object Deserialize(RestResponse response, Type type)
        {
            // do not try to deserialize if this is an error response
            if (response.ErrorException != null)
            {
                return GetDefaultValue(type);
            }

            if (type == typeof(byte[])) // return byte array
            {
                return response.RawBytes;
            }

            // TODO: ? if (type.IsAssignableFrom(typeof(Stream)))
            if (type == typeof(Stream))
            {
                var bytes = response.RawBytes;
                if (response.Headers != null)
                {
                    var filePath = string.IsNullOrEmpty(_configuration.TempFolderPath)
                        ? Path.GetTempPath()
                        : _configuration.TempFolderPath;
                    var regex = new Regex(@"Content-Disposition=.*filename=['""]?([^'""\s]+)['""]?$");
                    foreach (var header in response.Headers)
                    {
                        var match = regex.Match(header.ToString());
                        if (match.Success)
                        {
                            string fileName = filePath + ClientUtils.SanitizeFilename(match.Groups[1].Value.Replace("\"", "").Replace("'", ""));
                            File.WriteAllBytes(fileName, bytes);
                            return new FileStream(fileName, FileMode.Open);
                        }
                    }
                }
                var stream = new MemoryStream(bytes);
                return stream;
            }

            if (type.Name.StartsWith("System.Nullable`1[[System.DateTime")) // return a datetime object
            {
                return DateTime.Parse(response.Content, null, System.Globalization.DateTimeStyles.RoundtripKind);
            }

            if (type == typeof(string) || type.Name.StartsWith("System.Nullable")) // return primitive type
            {
                return Convert.ChangeType(response.Content, type);
            }

            // at this point, it must be a model (json)
            try
            {
                return JsonConvert.DeserializeObject(response.Content, type, _serializerSettings);
            }
            catch (Exception e)
            {
                throw new ApiException(500, e.Message);
            }
        }

        private static object GetDefaultValue(Type type)
        {
            if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public ISerializer Serializer => this;
        public IDeserializer Deserializer => this;

        public string[] AcceptedContentTypes => ContentType.JsonAccept;

        public SupportsContentType SupportsContentType => contentType =>
            contentType.Value.EndsWith("json", StringComparison.InvariantCultureIgnoreCase) ||
            contentType.Value.EndsWith("javascript", StringComparison.InvariantCultureIgnoreCase);

        public ContentType ContentType
        {
            get { return _contentType; }
            set { throw new InvalidOperationException("Not allowed to set content type."); }
        }
        
        public Finbourne.Sdk.Core.RestSharp.DataFormat DataFormat => Finbourne.Sdk.Core.RestSharp.DataFormat.Json;
    }

    /// <summary>
    /// The methods which must be provided by the http client
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Execute the http request synchronously
        /// </summary>
        Response<T> Execute<T>(Request request);
        
        /// <summary>
        /// Executes the http request asynchronously
        /// </summary>
        Task<Response<T>> ExecuteAsync<T>(Request request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// A wrapper around required RestClient methods
    /// </summary>
    internal class RestClientWrapper : RestClient, IClient
    {
        private class RestSharpAuthenticator : Finbourne.Sdk.Core.RestSharp.Authenticators.IAuthenticator
        {
            private readonly OAuthAuthenticator _authenticator;

            public RestSharpAuthenticator(OAuthAuthenticator authenticator)
            {
                _authenticator = authenticator;
            }

            public async ValueTask Authenticate(IRestClient client, RestRequest request)
            {
                await _authenticator.Authenticate(request.ToSdkRequest());
            }
        }
        
        public RestClientWrapper(
            HttpClient httpClient,
            ClientOptions? options,
            bool disposeHttpClient = false,
            ConfigureSerialization? configureSerialization = null
        ) : base(
            httpClient, 
            new RestClientOptions
            {
                BaseUrl = new Uri(options?.BaseUrl),
                Authenticator = options?.Authenticator != null ? new RestSharpAuthenticator(options.Authenticator) : null,
                ClientCertificates = options?.ClientCertificates,
                Proxy = options?.Proxy,
                UserAgent = options?.UserAgent,
                Timeout = options?.Timeout
            }, 
            disposeHttpClient, 
            configureSerialization)
        {
        }

        public Response<T> Execute<T>(Request request)
        {
            var restSharpRequest = request.ToRestSharpRequest();
            var restResponse = this.Execute<T>(restSharpRequest);
            return restResponse.ToSdkResponse();
        }

        public async Task<Response<T>> ExecuteAsync<T>(Request request, CancellationToken cancellationToken = default)
        {
            var restSharpRequest = request.ToRestSharpRequest();
            var restResponse = await this.ExecuteAsync<T>(restSharpRequest, cancellationToken);
            return restResponse.ToSdkResponse();
        }
    }

    /// <summary>
    /// Provides a default implementation of an Api client (both synchronous and asynchronous implementations),
    /// encapsulating general REST accessor use cases.
    /// </summary>
    public partial class ApiClient : ISynchronousClient, IAsynchronousClient
    {
        private readonly string _baseUrl;
        private readonly Func<ClientOptions, HttpMessageHandler> _createHttpMessageHandler;
        private readonly bool _disposeHandler;
        private readonly Func<ClientOptions, IReadableConfiguration, IClient> _createRestClient;

        /// <summary>
        /// Specifies the settings on a <see cref="JsonSerializer" /> object.
        /// These settings can be adjusted to accommodate custom serialization rules.
        /// </summary>
        public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings
        {
            // OpenAPI generated types generally hide default constructors.
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    OverrideSpecifiedNames = false
                }
            },
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        /// <summary>
        /// Allows for extending request processing for <see cref="ApiClient"/> generated code.
        /// </summary>
        /// <param name="request">The request object</param>
        partial void InterceptRequest(Request request);

        /// <summary>
        /// Allows for extending response processing for <see cref="ApiClient"/> generated code.
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="response">The response object</param>
        partial void InterceptResponse(Request request, ResponseBase response);

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient" />, defaulting to the global configurations' base url.
        /// </summary>
        public ApiClient():this(Client.GlobalConfiguration.Instance.BasePath)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient" />
        /// </summary>
        /// <param name="basePath">The target service's base path in URL format.</param>
        /// <param name="CreateHttpMessageHandler">A function to create a HttpMessageHandler for each REST request</param>
        /// <param name="disposeHandler">Should the HttpMessageHandler be disposed of after each request</param>
        /// <param name="createRestClientFunc">A function that given configuration will return an implementation of the IRestClientWrapper interface. Use to override the default RestClient</param>
        /// <exception cref="ArgumentException"></exception>
        public ApiClient(
            string basePath, 
            Func<ClientOptions, HttpMessageHandler>? CreateHttpMessageHandler = null, 
            bool disposeHandler = true,
            Func<ClientOptions, IReadableConfiguration, IClient> createRestClientFunc = null){
            if (string.IsNullOrEmpty(basePath))
                throw new ArgumentException("basePath cannot be empty");
            _baseUrl = basePath;
            _createHttpMessageHandler = CreateHttpMessageHandler ?? TcpKeepAlive.CreateTcpKeepAliveMessageHandler;
            _disposeHandler = disposeHandler;
            _createRestClient = createRestClientFunc ?? DefaultCreateRestClient;
        }

        private IClient DefaultCreateRestClient(ClientOptions clientOptions, IReadableConfiguration configuration)
        {
            var httpClient = new HttpClient(_createHttpMessageHandler(clientOptions), _disposeHandler);
            // set the timeout to be infinite, because the timeout is set on the request and then the lower of these two times
            // will be used. It would therefore not be possible to set an infinite timeout if we didn't do it here
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
            return new RestClientWrapper(httpClient,
                options: clientOptions,
                configureSerialization: s =>
                    s.UseSerializer(() => new CustomJsonCodec(SerializerSettings, configuration)));
        }


        /// <summary>
        /// Provides all logic for constructing a new Finbourne.Sdk.Core.RestSharp <see cref="RestRequest"/>.
        /// At this point, all information for querying the service is known. Here, it is simply
        /// mapped into the Finbourne.Sdk.Core.RestSharp request.
        /// </summary>
        /// <param name="method">The http verb.</param>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>[private] A new RestRequest instance.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private Request NewRequest(
            HttpMethod method,
            string path,
            RequestOptions options,
            IReadableConfiguration configuration)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (options == null) throw new ArgumentNullException("options");
            if (configuration == null) throw new ArgumentNullException("configuration");

            var headers = options.HeaderParameters ?? new();
            if (configuration.DefaultHeaders != null)
            {
                foreach (var header in configuration.DefaultHeaders)
                {
                    headers.Add(header.Key, header.Value);
                }
            }
            Request request = new Request
            {
                Resource = path,
                Method = method,
                PathParameters = options.PathParameters ?? new(),
                QueryParameters = options.QueryParameters ?? new(),
                FormParameters = options.FormParameters ?? new(),
                FileParameters = options.FileParameters ?? new(),
                Headers = headers,
            };

            if (options.Data != null)
            {
                if (options.Data is Stream stream)
                {
                    AddContentTypeHeaderIfMissing(options, request, "application/octet-stream");
                    request.RequestFormat = DataFormat.Binary;
                    request.Body = ClientUtils.ReadAsBytes(stream);
                }
                else if (options.Data is byte[])
                {
                    AddContentTypeHeaderIfMissing(options, request, "application/octet-stream");
                    request.RequestFormat = DataFormat.Binary;
                    request.Body = options.Data;
                }
                else
                {
                    request.RequestFormat = DataFormat.Json;
                    request.Body = options.Data;
                }
            }

            request.Timeout = TimeSpan.FromMilliseconds(options.TimeoutMs ?? configuration.TimeoutMs);
            return request;
        }

        private static void AddContentTypeHeaderIfMissing(RequestOptions options, Request request, string contentType)
        {
            if (options.HeaderParameters == null || !options.HeaderParameters.ContainsKey("Content-Type"))
            {
                request.Headers.Add("Content-Type", contentType);
            }
        }

        private ApiResponse<T> ToApiResponse<T>(Response<T> response)
        {
            T result = response.Data;
            string rawContent = response.Content;
            
            var errorDescription = response.ErrorException?.ToString() ?? response.ErrorMessage;
            var transformed = new ApiResponse<T>(response.StatusCode, new Multimap<string, string>(StringComparer.OrdinalIgnoreCase), result, rawContent)
            {
                ErrorText = errorDescription
            };

            if (response.Headers != null)
            {
                foreach (var responseHeader in response.Headers)
                {
                    transformed.Headers.Add(responseHeader.Name, ClientUtils.ParameterToString(responseHeader.Value));
                }
            }
            
            return transformed;
        }

        private ApiResponse<T> Exec<T>(Request request, RequestOptions options, IReadableConfiguration configuration)
        {
            var baseUrl = configuration.GetOperationServerUrl(options.Operation, options.OperationIndex) ?? _baseUrl;
            
            var clientOptions = new ClientOptions(baseUrl)
            {
                ClientCertificates = configuration.ClientCertificates,
                Timeout = TimeSpan.FromMilliseconds(options.TimeoutMs ?? configuration.TimeoutMs),
                Proxy = configuration.Proxy,
                UserAgent = configuration.UserAgent
            };

            if (!string.IsNullOrEmpty(configuration.OAuthTokenUrl) &&
                !string.IsNullOrEmpty(configuration.OAuthClientId) &&
                !string.IsNullOrEmpty(configuration.OAuthClientSecret) &&
                configuration.OAuthFlow != null)
            {
                clientOptions.Authenticator = new OAuthAuthenticator(
                    configuration.OAuthTokenUrl,
                    configuration.OAuthClientId,
                    configuration.OAuthClientSecret,
                    configuration.OAuthFlow,
                    SerializerSettings,
                    configuration);
            }

            var client = _createRestClient(clientOptions, configuration);

            InterceptRequest(request);

            Response<T> response;

            var policy = GetSyncPolicy(options);
            if (policy != null)
            {
                var policyResult = policy.ExecuteAndCapture(() => client.Execute<T>(request));
                response = policyResult.Result as Response<T>;
                if (response == null)
                {
                    throw new Exception($"error casting response to {typeof(Response<T>)}");
                }
            }
            else
            {
                response = client.Execute<T>(request);
            }

            if (response.IsSuccessful)
            {
                // if the response type is oneOf/anyOf, call FromJSON to deserialize the data
                if (typeof(Finbourne.Configuration.Sdk.Model.AbstractOpenAPISchema).IsAssignableFrom(typeof(T)))
                {
                    try
                    {
                        response.Data = (T) typeof(T).GetMethod("FromJson").Invoke(null, new object[] { response.Content });
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException != null ? ex.InnerException : ex;
                    }
                }
                else if (typeof(T).Name == "Stream") // for binary response
                {
                    response.Data = (T)(object)new MemoryStream(response.RawBytes);
                }
                else if (typeof(T).Name == "Byte[]") // for byte response
                {
                    response.Data = (T)(object)response.RawBytes;
                }
                else if (typeof(T).Name == "String") // for string response
                {
                    response.Data = (T)(object)response.Content;
                }
            }

            InterceptResponse(request, response);

            return ToApiResponse(response);
        }

        /// <summary>
        /// Returns the policy configured for executing synchronous requests
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static Policy<ResponseBase>? GetSyncPolicy(RequestOptions options)
        {
            Policy<ResponseBase>? policy = RetryConfiguration.RetryPolicy ?? (RetryConfiguration.GetRetryPolicyFunc == null
                ? null
                : RetryConfiguration.GetRetryPolicyFunc(options));
            return policy;
        }

        private async Task<ApiResponse<T>> ExecAsync<T>(Request request, RequestOptions options, IReadableConfiguration configuration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var baseUrl = configuration.GetOperationServerUrl(options.Operation, options.OperationIndex) ?? _baseUrl;

            var clientOptions = new ClientOptions(baseUrl)
            {
                ClientCertificates = configuration.ClientCertificates,
                Timeout = TimeSpan.FromMilliseconds(options.TimeoutMs ?? configuration.TimeoutMs),
                Proxy = configuration.Proxy,
                UserAgent = configuration.UserAgent
            };

            if (!string.IsNullOrEmpty(configuration.OAuthTokenUrl) &&
                !string.IsNullOrEmpty(configuration.OAuthClientId) &&
                !string.IsNullOrEmpty(configuration.OAuthClientSecret) &&
                configuration.OAuthFlow != null)
            {
                clientOptions.Authenticator = new OAuthAuthenticator(
                    configuration.OAuthTokenUrl,
                    configuration.OAuthClientId,
                    configuration.OAuthClientSecret,
                    configuration.OAuthFlow,
                    SerializerSettings,
                    configuration);
            }
            
            var client = _createRestClient(clientOptions, configuration);
            InterceptRequest(request);

            Response<T> response;
            var policy = GetAsyncPolicy(options);
            
            if (policy != null)
            {
                Func<CancellationToken, Task<ResponseBase>> action = async ct => await client.ExecuteAsync<T>(request, ct);
                var policyResult = await policy.ExecuteAndCaptureAsync(action, cancellationToken).ConfigureAwait(false);
                response = policyResult.Result as Response<T>;
                if (response == null)
                {
                    throw new Exception($"error casting response to {typeof(Response<T>)}");
                }
            }
            else
            {
                response = await client.ExecuteAsync<T>(request, cancellationToken).ConfigureAwait(false);
            }

            if (response.IsSuccessful)
            {
                // if the response type is oneOf/anyOf, call FromJSON to deserialize the data
                if (typeof(Finbourne.Configuration.Sdk.Model.AbstractOpenAPISchema).IsAssignableFrom(typeof(T)))
                {
                    response.Data = (T) typeof(T).GetMethod("FromJson").Invoke(null, new object[] { response.Content });
                }
                else if (typeof(T).Name == "Stream") // for binary response
                {
                    response.Data = (T)(object)new MemoryStream(response.RawBytes);
                }
                else if (typeof(T).Name == "Byte[]") // for byte response
                {
                    response.Data = (T)(object)response.RawBytes;
                }
            }

            InterceptResponse(request, response);

            return ToApiResponse(response);
        }

        /// <summary>
        /// Returns the policy configured for executing asynchronous requests
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static AsyncPolicy<ResponseBase>? GetAsyncPolicy(RequestOptions options)
        {
            AsyncPolicy<ResponseBase>? policy = RetryConfiguration.AsyncRetryPolicy ?? (RetryConfiguration.GetAsyncRetryPolicyFunc == null
                ? null
                : RetryConfiguration.GetAsyncRetryPolicyFunc(options));
            return policy;
        }

        #region IAsynchronousClient
        /// <summary>
        /// Make a HTTP GET request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> GetAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Get, path, options, config), options, config, cancellationToken);
        }

        /// <summary>
        /// Make a HTTP POST request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> PostAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Post, path, options, config), options, config, cancellationToken);
        }

        /// <summary>
        /// Make a HTTP PUT request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> PutAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Put, path, options, config), options, config, cancellationToken);
        }

        /// <summary>
        /// Make a HTTP DELETE request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> DeleteAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Delete, path, options, config), options, config, cancellationToken);
        }

        /// <summary>
        /// Make a HTTP HEAD request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> HeadAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Head, path, options, config), options, config, cancellationToken);
        }

        /// <summary>
        /// Make a HTTP OPTION request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> OptionsAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Options, path, options, config), options, config, cancellationToken);
        }

        /// <summary>
        /// Make a HTTP PATCH request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> PatchAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Patch, path, options, config), options, config, cancellationToken);
        }
        #endregion IAsynchronousClient

        #region ISynchronousClient
        /// <summary>
        /// Make a HTTP GET request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Get<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Get, path, options, config), options, config);
        }

        /// <summary>
        /// Make a HTTP POST request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Post<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Post, path, options, config), options, config);
        }

        /// <summary>
        /// Make a HTTP PUT request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Put<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Put, path, options, config), options, config);
        }

        /// <summary>
        /// Make a HTTP DELETE request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Delete<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Delete, path, options, config), options, config);
        }

        /// <summary>
        /// Make a HTTP HEAD request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Head<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Head, path, options, config), options, config);
        }

        /// <summary>
        /// Make a HTTP OPTION request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Options<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Options, path, options, config), options, config);
        }

        /// <summary>
        /// Make a HTTP PATCH request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Patch<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Patch, path, options, config), options, config);
        }
        #endregion ISynchronousClient
    }
}
