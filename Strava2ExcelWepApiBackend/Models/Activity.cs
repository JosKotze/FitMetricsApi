namespace Strava2ExcelWebApiBackend.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public long ActivityId { get; set; }
        public int UserId { get; set; }
        public string? Pace { get; set; }
        public string? Name { get; set; }
        public double Distance { get; set; }
        public int MovingTime { get; set; }
        public double? AverageHeartrate { get; set; }
        public double AverageSpeed { get; set; }
        public double TotalElevationGain { get; set; }
        public string? Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StartDateLocal { get; set; }
        public string? Timezone { get; set; }
        public double? MaxHeartrate { get; set; }
        //public ActivityDetails? ActivityDetails { get; set; }
    }
}
