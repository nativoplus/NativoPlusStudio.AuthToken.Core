using NativoPlusStudio.AuthToken.HttpClients.Interface;

namespace NativoPlusStudio.AuthToken.HttpClients.DTOs
{
    public class TokenResponse : ITokenResponse
    {
        public string Token { get; set; }
        public string EncryptedToken { get; set; }
        public string TokenType { get; set; }
        public string ExpiresIn { get; set; }
    }
}
