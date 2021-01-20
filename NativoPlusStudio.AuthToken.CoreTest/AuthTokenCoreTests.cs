using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NativoPlusStudio.AuthToken.Core.Extensions;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.AuthToken.DTOs;
using Newtonsoft.Json;
using System;

namespace NativoPlusStudio.AuthToken.CoreTest
{
    [TestClass]
    public class AuthTokenCoreTests : BaseConfiguration
    {
        [TestMethod]
        public void ExtensionTests()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjE1Nzc4NDc2MDB9.aKwzUWYaKzJb-uFnQl7yDPqMMfXeGFE08j4C7TGBMm4" ;
            var date = token.BuildJwtSecurityToken()
                .GetClaimFromJwtToken("exp")
                .GetExpirationDateInUtcFromJwsTokenClaim();
            Assert.IsTrue(date.HasValue);
        }
        
        [TestMethod]
        public void AuthTokenGeneratorTest()
        {
            IAuthTokenGenerator authTokenGenerator;

            authTokenGenerator = serviceProvider.GetRequiredService<IAuthTokenGenerator>();

            var token = authTokenGenerator?.GetTokenAsync(protectedResource: implementationName).GetAwaiter().GetResult();
            var tokenResp = new TokenResponse() { EncryptedToken = "thisismyencryptedtoken", ExpiryDateUtc = DateTime.MaxValue, Token = "thisismytoken", TokenType = "Bearer" };
            var tokenStr1 = JsonConvert.SerializeObject(token);
            var tokenStr2 = JsonConvert.SerializeObject(tokenResp);
            Assert.IsTrue(tokenStr1 == tokenStr2);
        }
    }
}
