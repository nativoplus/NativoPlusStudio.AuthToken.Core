using NativoPlusStudio.AuthToken.Core.Enums;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NativoPlusStudio.AuthToken.Core.Extensions
{
    public static class HttpClientPolicyHelperExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> CreateTokenRefreshPolicyOnUnauthorized(this IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            int retryCount)
        {
            if (generator == null) throw new ArgumentNullException("IAuthTokenGenerator generator", "Must be initialized");

            return generator
                .RetryPolicy(
                    message, 
                    protectedResource, 
                    asyncAction, 
                    retryCount,
                    HttpStatusCode.Unauthorized
                );
        }

        public static AsyncRetryPolicy<HttpResponseMessage> CreateTokenRefreshPolicyOnUnauthorized(this IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            BackoffAlgorithmTypeEnums backOffType,
            int initialDelayInSeconds,
            int retryCount)
        {
            if (generator == null) throw new ArgumentNullException("IAuthTokenGenerator generator", "Must be initialized");

            return generator
                .WaitAndRetryPolicyWithBackoff(
                    message, 
                    protectedResource, 
                    asyncAction, 
                    backOffType.GenerateBackoffDelay(initialDelayInSeconds, retryCount),
                    HttpStatusCode.Unauthorized
                );
        }

        public static AsyncRetryPolicy<HttpResponseMessage> RetryPolicy(this IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            int retryCount,
            params HttpStatusCode[] httpStatusCodes
            )
        {
            httpStatusCodes = httpStatusCodes.Any() ? httpStatusCodes : new HttpStatusCode[] { HttpStatusCode.Unauthorized };
            return Policy
                .HandleResult<HttpResponseMessage>(message => httpStatusCodes.Contains(message.StatusCode))
                .RetryAsync(retryCount, async (result, retryCount, context) =>
                {
                    await asyncAction.Invoke(generator, message, protectedResource);
                });
        }

        public static AsyncRetryPolicy<HttpResponseMessage> WaitAndRetryPolicyWithBackoff(this IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            IEnumerable<TimeSpan> delay,
            params HttpStatusCode[] httpStatusCodes
            )
        {
            httpStatusCodes = httpStatusCodes.Any() ? httpStatusCodes : new HttpStatusCode[] { HttpStatusCode.Unauthorized };

            return Policy
                .HandleResult<HttpResponseMessage>(message => httpStatusCodes.Contains(message.StatusCode))
                .WaitAndRetryAsync(delay, async (result, retryCount, context) =>
                {
                    await asyncAction.Invoke(generator, message, protectedResource);
                });
        }

    }
}
