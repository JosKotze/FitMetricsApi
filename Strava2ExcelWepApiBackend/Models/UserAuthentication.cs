namespace Strava2ExcelWebApiBackend.Models
{
    public class UserAuthentication
    {
        public int Id { get; set; }
        public string passwordHash { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public DateTime? refreashTokenCreated { get; set; }
    }
}
