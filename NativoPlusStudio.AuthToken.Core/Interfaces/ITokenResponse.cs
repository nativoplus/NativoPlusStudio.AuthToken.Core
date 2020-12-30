﻿namespace NativoPlusStudio.AuthToken.Core.Interfaces
{
    public interface ITokenResponse
    {
        string Token { get; set; }
        string TokenType { get; set; }
        string ExpiresIn { get; set; }
        string EncryptedToken { get; set; }

    }
}