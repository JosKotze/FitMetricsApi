namespace Strava2ExcelWebApiBackend.Models
{

    public class ActivityDetails
    {
        public int Id { get; set; }              // Primary key
        public long ActivityId { get; set; }     // Strava activity ID

        public int UserId { get; set; }
        public string? ExternalId { get; set; }
        public long? UploadId { get; set; }
        public int AthleteId { get; set; }
        public string? Name { get; set; }
        public double Distance { get; set; }
        public int MovingTime { get; set; }
        public int ElapsedTime { get; set; }
        public double TotalElevationGain { get; set; }
        public string? Type { get; set; }
        public string? SportType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StartDateLocal { get; set; }
        public string? Timezone { get; set; }
        public double? UtcOffset { get; set; }
        public List<double>? StartLatlng { get; set; }
        public List<double>? EndLatlng { get; set; }
        public int AchievementCount { get; set; }
        public int KudosCount { get; set; }
        public int CommentCount { get; set; }
        public int AthleteCount { get; set; }
        public int PhotoCount { get; set; }
        public int TotalPhotoCount { get; set; }
        public string? MapId { get; set; }
        public string? Polyline { get; set; }
        public string? SummaryPolyline { get; set; }
        public bool Trainer { get; set; }
        public bool Commute { get; set; }
        public bool Manual { get; set; }
        public bool Private { get; set; }
        public bool Flagged { get; set; }
        public string? Visibility { get; set; }
        public string? GearId { get; set; }
        public string? DeviceName { get; set; }
        public string? EmbedToken { get; set; }
        public double AverageSpeed { get; set; }
        public double MaxSpeed { get; set; }
        public double? AverageCadence { get; set; }
        public double? AverageWatts { get; set; }
        public double? AverageHeartrate { get; set; }
        public double? MaxHeartrate { get; set; }
        public int? WeightedAverageWatts { get; set; }
        public double? Kilojoules { get; set; }
        public bool? DeviceWatts { get; set; }
        public int? MaxWatts { get; set; }
        public double? ElevHigh { get; set; }
        public double? ElevLow { get; set; }
        public int PrCount { get; set; }
        public double? Calories { get; set; }
        public bool HasKudoed { get; set; }
        public int? WorkoutType { get; set; }
        public string? Description { get; set; }
        public int ResourceState { get; set; }
        public string? SegmentEffortsJson { get; set; }
        public string? SplitsMetricJson { get; set; }
        public string? LapsJson { get; set; }
    }

}
