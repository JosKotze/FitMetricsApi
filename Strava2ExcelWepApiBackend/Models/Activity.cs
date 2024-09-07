namespace Strava2ExcelWebApiBackend.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string? Pace { get; set; }
        public string? Name { get; set; }
        public double Distance { get; set; }
        public int MovingTime { get; set; }
        public double TotalElevationGain { get; set; }
        public string? Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StartDateLocal { get; set; }
        public string? Timezone { get; set; }
        public double? UtcOffset { get; set; }
        public string? LocationCity { get; set; }
        public string? LocationState { get; set; }
        public string? LocationCountry { get; set; }
        public int? AchievementCount { get; set; }
        public double AverageSpeed { get; set; }
        public double MaxSpeed { get; set; }
        public double? AverageWatts { get; set; }
        public double? Kilojoules { get; set; }
        public bool? DeviceWatts { get; set; }
        public double? AverageHeartrate { get; set; }
        public double? MaxHeartrate { get; set; }
        public int UserId { get; set; }
    }

}
