using System;

namespace NativoPlusStudio.AuthToken.Core.Interfaces
{
    public interface ITokenResponse
    {
        string Token { get; set; }
        string TokenType { get; set; }
        DateTime? ExpiryDateUtc { get; set; }
        string EncryptedToken { get; set; }

    }
}