using Microsoft.Extensions.DependencyInjection;

namespace NativoPlusStudio.AuthToken.Core.Interfaces
{
    public interface IAuthTokenProviderBuilder
    {
        IServiceCollection Services { get; set; }
    }
}