using System.Threading.Tasks;

namespace NativoPlusStudio.AuthToken.HttpClients.Interface
{
    public interface IAuthTokenGenerator
    {
        Task<ITokenResponse> GetTokenAsync(string protectedResource, bool includeEncryptedTokenInResponse = false);
    }
}