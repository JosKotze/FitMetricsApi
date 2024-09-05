//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Strava2ExcelWebApiBackend.Data;
//using Strava2ExcelWebApiBackend.Interfaces;
//using Strava2ExcelWebApiBackend.Models;
//using System.Collections.Concurrent;

//namespace Strava2ExcelWebApiBackend.Services
//{
//    public class UserService : IUserService
//    {
//        private readonly StravaDbContext _dbContext;

//        public UserService(StravaDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public async Task<bool> CreateUserAsync(string name, string surname,string email, string password)
//        {
//            if (await _dbContext.Athletes.AnyAsync(u => u.Email == email))
//            {
//                return false; // User already exists
//            }

//            var user = new User
//            {
//                Name = name,
//                Surname = surname,
//                Email = email,
//                PasswordHash = HashPassword(password) // Implement HashPassword method
//            };

//            _dbContext.Athletes.Add(user);
//            await _dbContext.SaveChangesAsync();
//            return true;
//        }

//        public async Task<bool> AuthenticateAsync(string username, string password)
//        {
//            var user = await _dbContext.Athletes
//                .FirstOrDefaultAsync(u => u.Email == username);

//            if (user == null || !VerifyPassword(user.PasswordHash, password)) // Implement VerifyPassword method
//            {
//                return false;
//            }

//            return true;
//        }

//        private string HashPassword(string password)
//        {
//            // Implement password hashing (e.g., using bcrypt)
//            return password; // Replace with actual hash
//        }

//        private bool VerifyPassword(string hashedPassword, string password)
//        {
//            // Implement password verification
//            return hashedPassword == password; // Replace with actual verification
//        }
//    }
//}
