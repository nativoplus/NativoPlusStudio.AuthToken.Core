using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace NativoPlusStudio.AuthToken.Core.Extensions
{
    public static class JwtHelperExtensions
    {
        public static JwtSecurityToken BuildJwtSecurityToken(this string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            return new JwtSecurityTokenHandler().ReadJwtToken(token);
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
