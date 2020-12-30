using System;

namespace NativoPlusStudio.AuthToken.Core.Interfaces
{
    public interface IAuthTokenDetails
    {
        /// <summary>
        /// 
        /// </summary>
        string ProtectedResourceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string TokenType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool IsExpired { get; set; }
    }
}
