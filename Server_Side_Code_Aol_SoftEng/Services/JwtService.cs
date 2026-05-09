using Microsoft.IdentityModel.Tokens;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server_Side_Code_Aol_SoftEng.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateSecurityToken(string username, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            try
            {
                var principal = getPrincipal(token);
                return principal != null;
            }
            catch
            {
                return false;
            }

        }
        public string GetUsernameFromToken(string token)
        {
            var principal = getPrincipal(token);
            var username = principal.Identity?.Name;
            if (username is null)
            {
                return "";
            }
            return username;
        }

        public string GetRoleFromToken(string token)
        {
            var principal = getPrincipal(token);
            var role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (role is null)
            {
                return "";
            }
            return role;
        }

        private ClaimsPrincipal getPrincipal(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenHandler = new JwtSecurityTokenHandler();
            var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

            var tokenParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = secureKey
            };

            var principal = tokenHandler.ValidateToken(token, tokenParameters, out var securityToken);
            return principal;
        }
    }
}
