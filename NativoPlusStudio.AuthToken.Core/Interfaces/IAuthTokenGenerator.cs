using System.Threading.Tasks;

namespace NativoPlusStudio.AuthToken.Core.Interfaces
{
    public interface IAuthTokenGenerator
    {
        Task<ITokenResponse> GetTokenAsync(string protectedResource);
    }
}