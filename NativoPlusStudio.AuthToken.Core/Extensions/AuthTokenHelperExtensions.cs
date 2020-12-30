using NativoPlusStudio.AuthToken.HttpClients.Interface;
using Polly;
using Polly.Retry;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NativoPlusStudio.AuthToken.Core.Extensions
{
    public static class AuthTokenHelperExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> CreateTokenRefreshPolicyOnUnauthorized(this IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            int retryCount = 1)
        {
            if (generator == null) throw new ArgumentNullException("IAuthTokenGenerator generator", "Must be initialized");

            var policy = Policy
                .HandleResult<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(retryCount, async (result, retryCount, context) =>
                {
                    await asyncAction.Invoke(generator, message, protectedResource);
                });

            return policy;
        }
        
        public static AsyncRetryPolicy<HttpResponseMessage> CreateTokenRefreshPolicy(this IAuthTokenGenerator generator,
            HttpRequestMessage message,
            string protectedResource,
            Func<IAuthTokenGenerator, HttpRequestMessage, string, Task> asyncAction,
            int retryCount = 1,
            params HttpStatusCode[] httpStatusCodes)
        {
            if (generator == null) throw new ArgumentNullException("IAuthTokenGenerator generator", "Must be initialized");

            var policy = Policy
                .HandleResult<HttpResponseMessage>(message => httpStatusCodes.Contains(message.StatusCode))
                .RetryAsync(retryCount, async (result, retryCount, context) =>
                {
                    await asyncAction.Invoke(generator, message, protectedResource);
                });

            return policy;
        }
        
        public static JwtSecurityToken BuildJwtSecurityToken(this ITokenResponse tokenResponse)
        {
            if (string.IsNullOrEmpty(tokenResponse?.Token))
            {
                return null;
            }

            return new JwtSecurityTokenHandler().ReadJwtToken(tokenResponse?.Token);
        }
        
        public static DateTime? GetExpirationDateInUtcFromJwsToken(this JwtSecurityToken jwtToken)
        {
            if(jwtToken == null)
            {
                return null;
            }

            var parsed = int.TryParse(jwtToken.Claims.Where(x => x.Type == "exp").FirstOrDefault().Value, out int epochSeconds);
            if (parsed)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(epochSeconds);
                return dateTimeOffset.UtcDateTime;
            }
            return null;
        }
    }
}
