using Microsoft.Extensions.DependencyInjection;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.Encryption.Interfaces;
using Serilog;

namespace NativoPlusStudio.AuthToken.Core
{
    public class AuthTokenServicesBuilder
    {
        public IAuthTokenCacheService TokenCacheService { get; private set; }
        public IEncryption EncryptionService { get; private set; }
        public ILogger Logger { get; private set; }
        public IServiceCollection Services { private get; set; }


        public AuthTokenServicesBuilder AddAuthTokenCacheImplementation(IAuthTokenCacheService service)
        {
            TokenCacheService = service;
            return this;
        }

        public AuthTokenServicesBuilder AddAuthTokenEncryptionImplementation(IEncryption symmetricEncryption)
        {
            EncryptionService = symmetricEncryption;
            return this;
        }
        
        public AuthTokenServicesBuilder AddLogger(ILogger logger)
        {
            Logger = logger;
            return this;
        }
        
        public AuthTokenServicesBuilder AddLogger()
        {
            Logger = Services.BuildServiceProvider().GetService<ILogger>();
            return this;
        }
    }
}
