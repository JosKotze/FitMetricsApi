using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Strava2ExcelWebApiBackend.Data;
using Strava2ExcelWebApiBackend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Strava2ExcelWebApiBackend.Controllers
{
    public class UserAuthController : ControllerBase
    {
        private readonly StravaDbContext _context;
        private readonly IConfiguration _configuration;

        public UserAuthController(StravaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(Athlete athlete)
        {
            if (await _context.Athletes.AnyAsync(u => u.Email == athlete.Email))
            {
                return BadRequest("User already exists");
            }

            var user = new Athlete
            {
                Name = athlete.Name,
                Surname = athlete.Surname,              
                AccessToken = athlete.AccessToken,
                Email = athlete.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(athlete.PasswordHash),
                RefreshTokenCreated = DateTime.Now,
                RefreshToken = athlete.RefreshToken,
            };

            _context.Athletes.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Token = GenerateJwtToken(user) });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var user = await _context.Athletes.SingleOrDefaultAsync(u => u.Email == userDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                return BadRequest("Invalid credentials");
            }

            return Ok(new { Token = GenerateJwtToken(user) });
        }

        private string GenerateJwtToken(Athlete user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
