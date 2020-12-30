using Microsoft.Extensions.DependencyInjection;
using NativoPlusStudio.AuthToken.Core.Enums;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NativoPlusStudio.AuthToken.Core.Extensions
{
    public static class HttpClientPolicyHelperExtensions
    {
        public static IHttpClientBuilder CreateTokenRefreshPolicyOnUnauthorized(this IHttpClientBuilder clientBuilder,
            IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            int retryCount)
        {
            if (generator == null) throw new ArgumentNullException("IAuthTokenGenerator generator", "Must be initialized");
            AsyncRetryPolicy<HttpResponseMessage> policy = generator.RetryOnAuthorization(message, protectedResource, asyncAction, retryCount);

            return clientBuilder.AddPolicyHandler(policy);
        }

        public static IHttpClientBuilder CreateTokenRefreshPolicyOnUnauthorized(this IHttpClientBuilder clientBuilder,
            IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            BackOffAlgorithmTypeEnums backOffType,
            int initialDelayInSeconds,
            int retryCount)
        {
            if (generator == null) throw new ArgumentNullException("IAuthTokenGenerator generator", "Must be initialized");
            AsyncRetryPolicy<HttpResponseMessage> policy = generator.WaitAndRetryWithBackOffOnAuthorization(message, protectedResource, asyncAction, backOffType.GenerateBackOffTimeSpans(initialDelayInSeconds, retryCount));

            return clientBuilder.AddPolicyHandler(policy);
        }

        public static AsyncRetryPolicy<HttpResponseMessage> RetryOnAuthorization(this IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            int retryCount)
        {
            return Policy
                .HandleResult<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(retryCount, async (result, retryCount, context) =>
                {
                    await asyncAction.Invoke(generator, message, protectedResource);
                });
        }

        public static AsyncRetryPolicy<HttpResponseMessage> WaitAndRetryWithBackOffOnAuthorization(this IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            IEnumerable<TimeSpan> delay
            )
        {
            return Policy
                .HandleResult<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.Unauthorized)
                .WaitAndRetryAsync(delay, async (result, retryCount, context) =>
                {
                    await asyncAction.Invoke(generator, message, protectedResource);
                });
        }

    }
}
