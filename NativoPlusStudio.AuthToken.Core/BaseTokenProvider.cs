using Microsoft.Extensions.Options;
using NativoPlusStudio.AuthToken.Core.DTOs;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.Encryption.Interfaces;
using Serilog;

namespace NativoPlusStudio.AuthToken.Core
{
    public abstract class BaseTokenProvider<TOption> where TOption : BaseOptions, new()
    {
        protected readonly IAuthTokenCacheService _tokenCacheService;
        protected readonly IEncryption _symmetricEncryption;
        protected readonly ILogger _logger;
        protected readonly TOption _options;

        public BaseTokenProvider(
            IEncryption symmetricEncryption,
            IAuthTokenCacheService tokenCacheService,
            ILogger logger,
            IOptions<TOption> options = null)
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
            
            if (options == null || options?.Value == null)
            {
                _options = new TOption();
            }
            else
            {
                _options = options.Value;
            }
            _symmetricEncryption = symmetricEncryption;
            _tokenCacheService = tokenCacheService;
        }
    }
}
