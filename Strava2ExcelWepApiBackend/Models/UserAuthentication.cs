namespace Strava2ExcelWebApiBackend.Models
{
    public class UserAuthentication
    {
        public Guid Id { get; set; }
        public Guid passwordHash { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public DateTime? refreashTokenCreated { get; set; }
    }
}
