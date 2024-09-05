using Strava2ExcelWebApiBackend.Models;

namespace Strava2ExcelWebApiBackend.Interfaces
{
    public interface IUserService
    {
        Task<bool> AuthenticateAsync(string username, string password);
        Task<bool> CreateUserAsync(string username, string password);
    }
}
