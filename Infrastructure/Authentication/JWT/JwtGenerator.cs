using Application.Interfaces.Authentication;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Authentication.JWT.Validator;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Infrastructure.Authentication.JWT
{
    public class JwtGenerator(IOptions<JwtSettings> jwtSettings) : IJwtGenerator
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public JwtToken GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new("sub", user.Id.ToString()),
                new("firstName", user.FirstName),
                new("lastName", user.LastName),
            };
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)), SecurityAlgorithms.HmacSha256);
            var securitytoken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Lifetime),
                signingCredentials: signingCredentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(securitytoken);
            return new JwtToken(token);
        }
    }
}
