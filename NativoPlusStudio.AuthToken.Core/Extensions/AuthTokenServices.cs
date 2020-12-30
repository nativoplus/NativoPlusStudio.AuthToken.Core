using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.Encryption.Interfaces;
using System;

namespace NativoPlusStudio.AuthToken.Core.Extensions
{
    public static class AuthTokenServices
    {
        public static IServiceCollection AddAuthTokenProvider(this IServiceCollection services, Action<IAuthTokenProviderBuilder> action) 
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<IAuthTokenGenerator, AuthTokenGenerator>();
            services.AddScoped<ITokenProvidersFactory, TokenProvidersFactory>();

            var builder = new AuthTokenProviderBuilder() { Services = services };

            action?.Invoke(builder);

            return services;
        }

        public static void AddTokenProviderHelper(this IAuthTokenProviderBuilder builder, string protectedResourceName, Action action = null) 
        {
            builder.Services.RemoveAll<IAuthTokenProvider>();
            builder.Services.RemoveAll<IEncryption>();
            builder.Services.RemoveAll<IAuthTokenCacheService>();

            action?.Invoke();

            var provider = builder.Services.BuildServiceProvider();
            var ficosoProvider = provider.GetRequiredService<IAuthTokenProvider>();
            var factoryProvider = provider.GetRequiredService<ITokenProvidersFactory>();
            factoryProvider.AddAuthTokenProvider(protectedResourceName, ficosoProvider);
            builder.Services.RemoveAll<ITokenProvidersFactory>();
            builder.Services.AddScoped<ITokenProvidersFactory, TokenProvidersFactory>(p => (TokenProvidersFactory)factoryProvider);
        }
    }
}
