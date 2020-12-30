using NativoPlusStudio.AuthToken.HttpClients.Interface;
using System.Collections.Generic;

namespace NativoPlusStudio.AuthToken.Core
{
    public class TokenProvidersFactory : ITokenProvidersFactory
    {
        private IDictionary<string, IAuthTokenProvider> _tokenProviderMap { get; set; }
        public TokenProvidersFactory()
        {
            _tokenProviderMap = new Dictionary<string, IAuthTokenProvider>();
        }

        public void AddAuthTokenProvider(string protectedResource, IAuthTokenProvider implementation)
        {
            if (_tokenProviderMap.ContainsKey(protectedResource))
            {
                _tokenProviderMap[protectedResource] = implementation;
            }
            else
            {
                _tokenProviderMap.Add(protectedResource, implementation);
            }
        }

        public IAuthTokenProvider GetTokenByAuthTokenProvider(string consortiumRawDataTable)
        {
            return _tokenProviderMap[consortiumRawDataTable];
        }

    }
}
