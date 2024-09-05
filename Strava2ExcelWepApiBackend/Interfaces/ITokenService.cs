using Strava2ExcelWebApiBackend.Models;

namespace Strava2ExcelWebApiBackend.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
