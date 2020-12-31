using NativoPlusStudio.AuthToken.Core.Enums;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using Polly;
using Polly.Fallback;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NativoPlusStudio.AuthToken.Core.Extensions
{
    public static class HttpClientPolicyHelperExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> CreateTokenRefreshPolicy(this IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            BackoffAlgorithmTypeEnums backOffType,
            int initialDelayInSeconds,
            int retryCount,
            Func<HttpResponseMessage, bool> condition = null)
        {
            if (generator == null) throw new ArgumentNullException("IAuthTokenGenerator generator", "Must be initialized");

            return generator
                .WaitAndRetryPolicyWithBackoff(
                    message, 
                    protectedResource, 
                    asyncAction, 
                    backOffType.GenerateBackoffDelay(initialDelayInSeconds, retryCount),
                    condition
                );
        }
        
        public static AsyncRetryPolicy<HttpResponseMessage> CreateTokenRefreshPolicy(this IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            int retryCount,
            Func<HttpResponseMessage, bool> condition = null)
        {
            if (generator == null) throw new ArgumentNullException("IAuthTokenGenerator generator", "Must be initialized");

            return generator
                .RetryPolicy(
                    message, 
                    protectedResource, 
                    asyncAction, 
                    retryCount,
                    condition
                );
        }
        
        public static AsyncRetryPolicy<HttpResponseMessage> RetryPolicy(this IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            int retryCount,
            Func<HttpResponseMessage, bool> condition
            )
        {
            return Policy
                .HandleResult<HttpResponseMessage>(message => condition?.Invoke(message) ?? message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
            Func<HttpResponseMessage, bool> condition
            )
        {
            return Policy
                .HandleResult<HttpResponseMessage>(message => condition?.Invoke(message) ?? message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                .WaitAndRetryAsync(delay, async (result, retryCount, context) =>
                {
                    await asyncAction.Invoke(generator, message, protectedResource);
                });
        }

        public static AsyncFallbackPolicy<HttpResponseMessage> AsyncFallbackPolicy(
            Func<HttpResponseMessage, bool> handleResultCondition,
            Func<HttpResponseMessage, Task<HttpResponseMessage>> fallbackValue
            )
        {
            return Policy<HttpResponseMessage>
                .HandleResult(message => handleResultCondition.Invoke(message))
                .FallbackAsync(async (delegateOutcome, context, token) =>
                {
                    return await fallbackValue.Invoke(delegateOutcome.Result);
                }, 
                async (delegateOutcome, context) => { /* log (if desired) that InternalServerError was checked for what kind */ });
        }
        
        public static FallbackPolicy<HttpResponseMessage> FallbackPolicy(
            Func<HttpResponseMessage, bool> handleResultCondition,
            Func<HttpResponseMessage, HttpResponseMessage> fallbackValue
            )
        {
            return Policy<HttpResponseMessage>
                .HandleResult(message => handleResultCondition.Invoke(message))
                .Fallback((delegateOutcome, context, token) =>
                {
                    return fallbackValue.Invoke(delegateOutcome.Result);
                }, (delegateOutcome, context) => { /* log (if desired) that InternalServerError was checked for what kind */ });
        }
    }
}
