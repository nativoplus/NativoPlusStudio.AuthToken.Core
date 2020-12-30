using System.Threading.Tasks;

namespace NativoPlusStudio.AuthToken.HttpClients.Interface
{
    public interface IAuthTokenProvider
    {
        Task<ITokenResponse> GetTokenAsync(bool includeEncryptedTokenInResponse = false);
    }
}