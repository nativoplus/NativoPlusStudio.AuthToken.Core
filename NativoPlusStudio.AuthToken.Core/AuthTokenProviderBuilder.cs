using Microsoft.Extensions.DependencyInjection;
using NativoPlusStudio.AuthToken.Core.Interfaces;

namespace NativoPlusStudio.AuthToken.Core
{
    public class AuthTokenProviderBuilder : IAuthTokenProviderBuilder
    {
        public IServiceCollection Services { get; set; }
    }
}
