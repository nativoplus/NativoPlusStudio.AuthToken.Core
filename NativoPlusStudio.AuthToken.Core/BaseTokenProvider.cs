using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.Encryption.Interfaces;
using Serilog;

namespace NativoPlusStudio.AuthToken.Core
{
    public abstract class BaseTokenProvider
    {
        protected readonly IAuthTokenCacheService _tokenCacheService;
        protected readonly IEncryption _symmetricEncryption;
        protected readonly ILogger _logger;

        public BaseTokenProvider(
            IEncryption symmetricEncryption,
            IAuthTokenCacheService tokenCacheService,
            ILogger logger)
        {
            if (logger == null)
            {
                _logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger();
            }
            else
            {
                _logger = logger;
            }
            _symmetricEncryption = symmetricEncryption;
            _tokenCacheService = tokenCacheService;
        }
    }
}
