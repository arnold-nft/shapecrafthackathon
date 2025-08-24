using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo.ShapeCraftAPI.Services
{
    public class TokenService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secret;

        public TokenService(IConfiguration config)
        {
            _issuer = config["Jwt:Issuer"]!;
            _audience = config["Jwt:Audience"]!;
            _secret = config["Jwt:Secret"]!;
        }

        public string GenerateToken(string userAddress)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userAddress),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


}
