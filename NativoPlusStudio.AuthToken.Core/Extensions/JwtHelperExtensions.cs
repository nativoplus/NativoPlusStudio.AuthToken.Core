using NativoPlusStudio.AuthToken.Core.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace NativoPlusStudio.AuthToken.Core.Extensions
{
    public static class JwtHelperExtensions
    {
        public static JwtSecurityToken BuildJwtSecurityToken(this ITokenResponse tokenResponse)
        {
            if (string.IsNullOrEmpty(tokenResponse?.Token))
            {
                return null;
            }

            return new JwtSecurityTokenHandler().ReadJwtToken(tokenResponse?.Token);
        }

        public static Claim GetClaimFromJwtToken(this JwtSecurityToken jwtToken, string claimType)
        {
            if (jwtToken == null)
            {
                return null;
            }

            return jwtToken.Claims.FirstOrDefault(x => x.Type == claimType);
            
        }
        
        public static DateTime? GetExpirationDateInUtcFromJwsTokenClaim(this Claim claim)
        {
            if (claim == null)
            {
                return null;
            }

            var parsed = int.TryParse(claim?.Value, out int epochSeconds);
            if (parsed)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(epochSeconds);
                return dateTimeOffset.UtcDateTime;
            }
            return null;
        }
    }
}
