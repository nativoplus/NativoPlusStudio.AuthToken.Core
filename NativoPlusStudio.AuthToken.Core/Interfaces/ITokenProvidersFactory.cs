namespace NativoPlusStudio.AuthToken.Core.Interfaces
{
    public interface ITokenProvidersFactory
    {
        void AddAuthTokenProvider(string protectedResource, IAuthTokenProvider implementation);

        IAuthTokenProvider GetTokenByAuthTokenProvider(string protectedResource);
        
    }
}