using System.ComponentModel.DataAnnotations;

namespace Strava2ExcelWebApiBackend.DTOs
{
    public class UserDto
    {
        public required string UserName { get; set; }
        public required string Token { get; set; }
    }
}
