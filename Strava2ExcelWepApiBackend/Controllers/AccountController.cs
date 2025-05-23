﻿using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Strava2ExcelWebApiBackend.Data;
using Strava2ExcelWebApiBackend.DTOs;
using Strava2ExcelWebApiBackend.Interfaces;
using Strava2ExcelWebApiBackend.Models;
using System.Security.Cryptography;
using System.Text;

namespace Strava2ExcelWebApiBackend.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/account")]
    public class AccountController(StravaDbContext context, ITokenService tokenService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username))
            {
                return BadRequest("A user with that username already exists in the system");
            }

            using var hmac = new System.Security.Cryptography.HMACSHA512(); // using 'using' to dispose it after use

            var user = new User
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            context.Athletes.Add(user);
            await context.SaveChangesAsync();

            return new UserDto
            {
                UserName = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await context.Athletes.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid user");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }

        //[HttpGet("startup")]
        //public IActionResult GetStartupData(string userName)
        //{
        //    accessToken = context.StravaAuth.FirstOrDefaultAsync(x => x.);

        //    var startupData = new Startup
        //    {
        //        UserId = 
        //        UserName = 
        //        AccessToken = 
        //        // Any other startup-related data
        //    };

            
        //}


        private async Task<bool> UserExists(string username)
        {
            return await context.Athletes.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }
    }
}
