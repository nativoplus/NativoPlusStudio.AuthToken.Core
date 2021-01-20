using Microsoft.Extensions.DependencyInjection;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.AuthToken.Core.Extensions;
using System;
using ExampleLib;

namespace NativoPlusStudio.AuthToken.CoreTest
{
    public abstract class BaseConfiguration
    {
        public static IServiceProvider serviceProvider;
        public static string implementationName = "NameOfImplementation";

        public BaseConfiguration()
        {
            var services = new ServiceCollection();
            services.AddTokenProviderHelper(
                protectedResourceName: implementationName,
                () =>
                {
                    //necessary code to register you own implementation

                    services.AddScoped<IAuthTokenProvider, ExampleTokenProvider>();
                    //services.AddHttpClient<IAuthTokenProvider, ExampleTokenProvider>();
                }
            );

            serviceProvider = services.BuildServiceProvider();
        }
    }
}
