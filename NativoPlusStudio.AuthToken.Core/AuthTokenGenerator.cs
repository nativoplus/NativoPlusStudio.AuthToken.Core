using NativoPlusStudio.AuthToken.Core.Interfaces;
using Serilog;
using System.Threading.Tasks;

namespace NativoPlusStudio.AuthToken.Core
{
    public class AuthTokenGenerator : IAuthTokenGenerator
    {
        private readonly ILogger _logger;
        private readonly ITokenProvidersFactory _tokenHttpClientFactory;
        public AuthTokenGenerator(ITokenProvidersFactory tokenHttpClientFactory, ILogger logger = null)
        {
            if(logger == null)
            {
                _logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger();
            }
            else
            {
                _logger = logger;
            }
            _tokenHttpClientFactory = tokenHttpClientFactory;
        }

        public async Task<ITokenResponse> GetTokenAsync(string protectedResource)
        {
            var tokenResponse = await _tokenHttpClientFactory
                .GetTokenByAuthTokenProvider(protectedResource)
                .GetTokenAsync();
            
            return tokenResponse;
        }
    }
}
