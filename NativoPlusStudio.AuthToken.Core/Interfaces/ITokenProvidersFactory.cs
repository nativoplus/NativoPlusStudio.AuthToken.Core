namespace NativoPlusStudio.AuthToken.HttpClients.Interface
{
    public interface ITokenProvidersFactory
    {
        void AddAuthTokenProvider(string protectedResource, IAuthTokenProvider implementation);

        IAuthTokenProvider GetTokenByAuthTokenProvider(string consortiumRawDataTable);
        
    }
}