namespace Strava2ExcelWebApiBackend.Models
{
    public class Athlete
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string PasswordHash { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreashTokenCreated { get; set; }

    }
}
