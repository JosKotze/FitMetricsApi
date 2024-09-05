//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using Strava2ExcelWebApiBackend.Data;
//using Strava2ExcelWebApiBackend.Models;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Strava2ExcelWebApiBackend.DTOs;
//using System.Security.Cryptography;
//using Strava2ExcelWebApiBackend.Interfaces;

//namespace Strava2ExcelWebApiBackend.Controllers
//{
//    public class UserAuthController(StravaDbContext context, IUserService userService, ITokenService tokenService) : ControllerBase
//    {

//        [HttpPost("register")]
//        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
//        {
//            if (await UserExists(registerDto.Username))
//            {
//                return BadRequest("A user with the email already exists");
//            }

//            using var hmac = new HMACSHA512();

//            var user = new User
//            {
//                UserName = registerDto.Username,
//                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
//                PasswordSalt = hmac.Key
//            };

//            context.Athletes.Add(user);
//            await context.SaveChangesAsync();

//            return new UserDto {
//                UserName = registerDto.Username,
//                Token = tokenService.CreateToken(user)
//            };
//        }






//        private async Task<bool> UserExists(string email)
//        {
//            return await context.Athletes.AnyAsync(x => x.UserName.ToLower() == email.ToLower());
//        }


//        //[HttpPost("signup")]
//        //public async Task<IActionResult> Signup([FromBody] SignupRequest request)
//        //{
//        //    if (await userService.CreateUserAsync(request.Name, request.Surname, request.Email, request.Password))
//        //    {
//        //        return Ok( new { message = "User registered successfully." });
//        //    }
//        //    return BadRequest( new { message = "User registration failed." });
//        //}

//        //[HttpPost("signupTest")]
//        //public async Task<IActionResult> signupTest(string name, string surname, string email, string password)
//        //{
//        //    if (await userService.CreateUserAsync(name, surname, email, password))
//        //    {
//        //        return Ok("User registered successfully.");
//        //    }
//        //    return BadRequest("User registration failed.");
//        //}

//        //[HttpPost("login")]
//        //public async Task<IActionResult> Login([FromBody] LoginRequest request)
//        //{
//        //    if (await userService.AuthenticateAsync(request.Email, request.Password))
//        //    {
//        //        return Ok( new { message = "Login successful." });
//        //    }
//        //    return Unauthorized(new { message = "Invalid username or password." });
//        //}

//        //[HttpPost("loginTest")]
//        //public async Task<IActionResult> LoginTest(string email, string password)
//        //{
//        //    if (await userService.AuthenticateAsync(email, password))
//        //    {
//        //        return Ok("Login successful.");
//        //    }
//        //    return Unauthorized("Invalid username or password.");
//        //}
//    }
//}
