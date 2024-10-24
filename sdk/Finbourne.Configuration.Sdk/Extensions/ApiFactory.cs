/*
 * FINBOURNE ConfigurationService API
 *
 * Contact: info@finbourne.com
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using Finbourne.Configuration.Sdk.Client;

namespace Finbourne.Configuration.Sdk.Extensions
{
    /// <summary>
    /// Factory to provide instances of the autogenerated Api
    /// </summary>
    public interface IApiFactory
    {
        /// <summary>
        /// Return the specific autogenerated Api
        /// </summary>
        TApi Api<TApi>() where TApi : class, IApiAccessor;
    }

    /// <inheritdoc />
    public class ApiFactory : IApiFactory
    {
        private static readonly IEnumerable<Type> ApiTypes = Assembly.GetAssembly(typeof(ApiClient))
            .GetTypes()
            .Where(t => typeof(IApiAccessor).IsAssignableFrom(t) && t.IsClass);

        private readonly IReadOnlyDictionary<Type, IApiAccessor> _apis;

        /// <summary>
        /// Create a new factory using the specified configuration
        /// </summary>
        /// <param name="apiConfiguration">Configuration for the ClientCredentialsFlowTokenProvider, usually sourced from a "secrets.json" file</param>
        public ApiFactory(ApiConfiguration apiConfiguration)
        {
            if (apiConfiguration == null) throw new ArgumentNullException(nameof(apiConfiguration));

            // Validate Uris
            // note: could employ a factory pattern here to create ITokenProvider in case more branching is required in the future:
            ITokenProvider tokenProvider;
            if (!string.IsNullOrWhiteSpace(apiConfiguration.PersonalAccessToken)) // the personal access token takes precedence over other methods of authentication
            {
                tokenProvider = new PersonalAccessTokenProvider(apiConfiguration.PersonalAccessToken);
            }
            else if (!string.IsNullOrWhiteSpace(apiConfiguration.AccessTokenOldName))
            {
                tokenProvider = new PersonalAccessTokenProvider(apiConfiguration.AccessTokenOldName);
            }
            else {
                if (!Uri.TryCreate(apiConfiguration.TokenUrl, UriKind.Absolute, out var _))
                {
                    throw new UriFormatException($"Invalid Token Uri: {apiConfiguration.TokenUrl}");
                }
                tokenProvider = new ClientCredentialsFlowTokenProvider(apiConfiguration); 
            }

            if (!Uri.TryCreate(apiConfiguration.BaseUrl, UriKind.Absolute, out var _))
            {
                if (string.IsNullOrWhiteSpace(apiConfiguration.BaseUrl))
                    throw new ArgumentNullException(
                        nameof(apiConfiguration.BaseUrl),
                        $"BaseUrl Uri missing. Please specify either FBN_CONFIGURATION_URL environment variable or configurationUrl in secrets.json.");

                throw new UriFormatException($"Invalid Uri: {apiConfiguration.BaseUrl}");
            }

            // Create configuration
            var configuration = new TokenProviderConfiguration(tokenProvider)
            {
                BasePath = apiConfiguration.BaseUrl
            };
            
            // if the timeout config has been specified, use this, else use the value from the global configuration
            configuration.TimeoutMs = apiConfiguration.TimeoutMs ?? GlobalConfiguration.Instance.TimeoutMs;
            
            // if the rate limit retry config has been specified, use this, else use the value from the global configuration
            configuration.RateLimitRetries = apiConfiguration.RateLimitRetries ?? GlobalConfiguration.Instance.RateLimitRetries;

            if(!String.IsNullOrWhiteSpace(apiConfiguration.ApplicationName))
            {
                configuration.DefaultHeaders.Add("X-LUSID-Application", apiConfiguration.ApplicationName);
            }
            configuration.MergeWithGlobalConfiguration();
            
            _apis = Init(configuration);
        }

        /// <summary>
        /// Create a new factory using the specified configuration
        /// </summary>
        /// <param name="configuration">A set of configuration settings</param>
        public ApiFactory(Client.Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configuration.MergeWithGlobalConfiguration();
            
            _apis = Init(configuration);
        }

        /// <inheritdoc />
        public TApi Api<TApi>() where TApi : class, IApiAccessor
        {
            _apis.TryGetValue(typeof(TApi), out var api);

            if (api == null)
            {
                throw new InvalidOperationException($"Unable to find api: {typeof(TApi)}");
            }

            return api as TApi;
        }

        private static Dictionary<Type, IApiAccessor> Init(Client.Configuration configuration)
        {
            // Set GetRetryPolicyFunc (if unset) unless RetryPolicy has been set 
            // Users can combine their own policy with the existing policies by using the .Wrap() method
            if (RetryConfiguration.RetryPolicy == null && RetryConfiguration.GetRetryPolicyFunc == null)
            {
                RetryConfiguration.GetRetryPolicyFunc = requestOptions =>
                {
                    var rateLimitRetries = requestOptions.RateLimitRetries ?? configuration.RateLimitRetries;
                    return PollyApiRetryHandler.GetDefaultRetryPolicyWithRateLimitWithFallback(rateLimitRetries);
                };
            }
            
            // Set GetAsyncRetryPolicyFunc (if unset) unless AsyncRetryPolicy has been set 
            // Users can combine their own policy with the existing policies by using the .WrapAsync() method
            if (RetryConfiguration.AsyncRetryPolicy == null && RetryConfiguration.GetAsyncRetryPolicyFunc == null)
            {
                RetryConfiguration.GetAsyncRetryPolicyFunc = requestOptions =>
                {
                    var rateLimitRetries = requestOptions.RateLimitRetries ?? configuration.RateLimitRetries;
                    return PollyApiRetryHandler.GetDefaultRetryPolicyWithRateLimitRetryWithFallbackAsync(rateLimitRetries);
                };
            }

            var dict = new Dictionary<Type, IApiAccessor>();
            foreach (Type api in ApiTypes)
            {
                if (!(Activator.CreateInstance(api, configuration) is IApiAccessor impl))
                {
                    throw new Exception($"Unable to create type {api}");
                }

                // Replace the default implementation of the ExceptionFactory with a custom one defined by FINBOURNE
                impl.ExceptionFactory = ExceptionHandler.CustomExceptionFactory;
                var @interface = api.GetInterfaces()
                    .First(i => typeof(IApiAccessor).IsAssignableFrom(i));

                dict[api] = impl;
                dict[@interface] = impl;
            }

            return dict;
        }
    }
}