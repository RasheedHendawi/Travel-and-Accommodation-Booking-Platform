using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace EndpointAPI.Tests.Helpers
{
    public  class TestAuthHelper
    {
        public static string GenerateTestToken(string userId, string role)
        {
            var key = new SymmetricSecurityKey("ThisIsA32ByteLongSecretKeyForHS256!"u8.ToArray());
            var credentials = new SigningCredentials(key, SecurityAlgorithms.Sha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, role)
        };

            var token = new JwtSecurityToken(
                issuer: "test-issuer",
                audience: "test-audience",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
