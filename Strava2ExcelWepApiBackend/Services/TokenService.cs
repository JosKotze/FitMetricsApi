using Microsoft.IdentityModel.Tokens;
using Strava2ExcelWebApiBackend.Interfaces;
using Strava2ExcelWebApiBackend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Strava2ExcelWebApiBackend.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(User user)
        {
            var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings");

            if (tokenKey.Length < 64) throw new Exception("Your tokenKey needs to be longer");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject (user ID)
                new Claim(JwtRegisteredClaimNames.Email, user.UserName), // Email
                new Claim(JwtRegisteredClaimNames.Iss, config["Jwt:Issuer"] ?? "FitMetricsProd"), // Issuer
                //new Claim(JwtRegisteredClaimNames.Aud, config["Jwt:Audience"] ?? "FitMetricsAPI"), // Audience
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64) // Issued at
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = creds,
                Issuer = config["Jwt:Issuer"] ?? "FitMetricsProd", // Explicit issuer
                Audience = config["Jwt:Audience"] ?? "FitMetricsAPIProd" // Explicit audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}