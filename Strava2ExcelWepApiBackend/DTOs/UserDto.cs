using System.ComponentModel.DataAnnotations;

namespace Strava2ExcelWebApiBackend.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Token { get; set; }
    }
}
