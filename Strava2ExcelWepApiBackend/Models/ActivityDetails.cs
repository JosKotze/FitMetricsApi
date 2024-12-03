namespace Strava2ExcelWebApiBackend.Models
{
    public class ActivityDetails
    {
        public int Id { get; set; }                  // Database primary key
        public long ActivityId { get; set; }         // Foreign key to Activity
        public string? Description { get; set; }     // Description of the activity
        public double? Calories { get; set; }        // Calories burned
        public bool? Trainer { get; set; }           // Was it a trainer activity?
        public double? AverageWatts { get; set; }    // Average power in watts
        public double? MaxWatts { get; set; }        // Max power in watts
        public double? WeightedAverageWatts { get; set; } // Weighted average watts
        public double? Kilojoules { get; set; }      // Total work in kilojoules
        public bool? DeviceWatts { get; set; }       // Device measured watts
        public double? ElevHigh { get; set; }        // Highest elevation point
        public double? ElevLow { get; set; }         // Lowest elevation point
        public string? DeviceName { get; set; }      // Name of the device used
        public string? EmbedToken { get; set; }      // Token for embedding
        public string? SegmentEfforts { get; set; }  // JSON string for segment efforts
        public string? SplitMetric { get; set; }     // JSON string for split metrics
        public string? Laps { get; set; }            // JSON string for laps
        public int KudosCount { get; set; }          // Kudos received
        public int CommentCount { get; set; }        // Comments received
        public int AchievementCount { get; set; }    // Number of achievements
        public int AthleteCount { get; set; }        // Number of athletes involved
        public string? SportType { get; set; }
        public List<double>? StartLatlng { get; set; }// Latitude/Longitude start coordinates
        public List<double>? EndLatlng { get; set; }
        //public Activity Activity { get; set; } = null!;
    }

}
