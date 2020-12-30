using Microsoft.VisualStudio.TestTools.UnitTesting;
using NativoPlusStudio.AuthToken.Core.Extensions;
using NativoPlusStudio.AuthToken.DTOs;

namespace NativoPlusStudio.AuthToken.CoreTest
{
    [TestClass]
    public class AuthTokenCoreTests
    {
        [TestMethod]
        public void ExtensionTests()
        {
            var token = new TokenResponse { Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjE1Nzc4NDc2MDB9.aKwzUWYaKzJb-uFnQl7yDPqMMfXeGFE08j4C7TGBMm4" };
            var date = token.BuildJwtSecurityToken()
                .GetClaimFromJwtToken("exp")
                .GetExpirationDateInUtcFromJwsTokenClaim();
            Assert.IsTrue(date.HasValue);
        }
    }
}
