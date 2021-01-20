using Microsoft.Extensions.DependencyInjection;
using System;
using NativoPlusStudio.AuthToken.Core.Extensions;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using Newtonsoft.Json;
using ExampleLib;

namespace Example
{
    class Program
    {
        public static IServiceProvider serviceProvider;
        public static IAuthTokenGenerator authTokenGenerator;
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            var implementationName = "NameOfImplementation";
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

            authTokenGenerator = serviceProvider.GetRequiredService<IAuthTokenGenerator>();

            var token = authTokenGenerator.GetTokenAsync(protectedResource: implementationName).GetAwaiter().GetResult();
            
            Console.WriteLine(JsonConvert.SerializeObject(token));
        }
    }
}
