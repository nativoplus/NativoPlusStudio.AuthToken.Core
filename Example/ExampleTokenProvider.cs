using Microsoft.Extensions.Options;
using NativoPlusStudio.AuthToken.Core;
using NativoPlusStudio.AuthToken.Core.DTOs;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.AuthToken.DTOs;
using NativoPlusStudio.Encryption.Interfaces;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Example
{
    public class ExampleTokenProvider : BaseTokenProvider<BaseOptions>, IAuthTokenProvider
    {
        public ExampleTokenProvider(IEncryption symmetricEncryption = null,
            IAuthTokenCacheService tokenCacheService = null,
            ILogger logger = null,
            IOptions<BaseOptions> options = null)
            :base(symmetricEncryption, tokenCacheService, logger, options)
        {

        }

        public async Task<ITokenResponse> GetTokenAsync()
        {

            //add optional code to get the token from a cache
            //add optional code to encrypt the token
            return new TokenResponse() { EncryptedToken = "thisismyencryptedtoken", ExpiryDateUtc = DateTime.UtcNow, Token = "thisismytoken", TokenType = "Bearer" };
        }
    }
}
