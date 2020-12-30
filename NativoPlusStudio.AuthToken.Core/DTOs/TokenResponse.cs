using NativoPlusStudio.AuthToken.Core.Interfaces;
using System;

namespace NativoPlusStudio.AuthToken.DTOs
{
    public class TokenResponse : ITokenResponse
    {
        public string Token { get; set; }
        public string EncryptedToken { get; set; }
        public string TokenType { get; set; }
        public DateTime? ExpiryDateUtc { get; set; }
    }
}
