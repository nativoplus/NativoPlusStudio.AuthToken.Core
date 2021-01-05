using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.Encryption.Interfaces;
using System;

namespace NativoPlusStudio.AuthToken.Core.Extensions
{
    public static class AuthTokenServices
    {
        public static void AddTokenProviderHelper(this IServiceCollection services, string protectedResourceName, Action action = null) 
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<IAuthTokenGenerator, AuthTokenGenerator>();
            services.AddScoped<ITokenProvidersFactory, TokenProvidersFactory>();

            services.RemoveAll<IAuthTokenProvider>();
            services.RemoveAll<IEncryption>();
            services.RemoveAll<IAuthTokenCacheService>();

            action?.Invoke();

            var provider = services.BuildServiceProvider();
            var ficosoProvider = provider.GetRequiredService<IAuthTokenProvider>();
            var factoryProvider = provider.GetRequiredService<ITokenProvidersFactory>();
            factoryProvider.AddAuthTokenProvider(protectedResourceName, ficosoProvider);
            services.RemoveAll<ITokenProvidersFactory>();
            services.AddScoped<ITokenProvidersFactory, TokenProvidersFactory>(p => (TokenProvidersFactory)factoryProvider);
        }
    }
}
