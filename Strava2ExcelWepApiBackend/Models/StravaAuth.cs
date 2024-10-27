namespace Strava2ExcelWebApiBackend.Models
{
    public class StravaAuth
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string accessToken { get; set; }
        public DateTime? expiresAt { get; set; }
    }
}
