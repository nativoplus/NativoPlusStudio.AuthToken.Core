using System;

namespace NativoPlusStudio.AuthToken.Core.Interfaces
{
    public interface IAuthTokenCacheService
    {
        IAuthTokenDetails GetCachedAuthToken(string protectedResourceName);
        (int upsertResult, string errorMessage) UpsertAuthTokenCache(string protectedResourceName, string token, string tokenType, DateTime? expirationDate);
    }
}
