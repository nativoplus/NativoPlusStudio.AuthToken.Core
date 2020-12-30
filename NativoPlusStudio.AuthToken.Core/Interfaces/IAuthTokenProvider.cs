using System.Threading.Tasks;

namespace NativoPlusStudio.AuthToken.Core.Interfaces
{
    public interface IAuthTokenProvider
    {
        Task<ITokenResponse> GetTokenAsync(bool includeEncryptedTokenInResponse = false);
    }
}