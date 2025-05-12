namespace Strava2ExcelWebApiBackend.Models
{
    public class ActivityDetails3
    {
        // Database primary key (assuming auto-incremented ID for your DB)
        public int Id { get; set; }

        // Core activity identifiers
        public long ActivityId { get; set; }          // Maps to "id" from JSON
        public string ExternalId { get; set; }        // Maps to "external_id"
        public long? UploadId { get; set; }           // Maps to "upload_id", nullable as it might not always be present

        // Athlete reference
        public int AthleteId { get; set; }            // Foreign key to Athlete table, maps to "athlete.id"

        // Basic activity details
        public string Name { get; set; }              // Maps to "name"
        public double Distance { get; set; }          // Maps to "distance"
        public int MovingTime { get; set; }           // Maps to "moving_time" (seconds)
        public int ElapsedTime { get; set; }          // Maps to "elapsed_time" (seconds)
        public double TotalElevationGain { get; set; }// Maps to "total_elevation_gain"
        public string Type { get; set; }              // Maps to "type"
        public string SportType { get; set; }         // Maps to "sport_type"

        // Date and location
        public DateTime? StartDate { get; set; }      // Maps to "start_date"
        public DateTime? StartDateLocal { get; set; } // Maps to "start_date_local"
        public string Timezone { get; set; }          // Maps to "timezone"
        public double? UtcOffset { get; set; }        // Maps to "utc_offset"
        public List<double> StartLatlng { get; set; } // Maps to "start_latlng"
        public List<double> EndLatlng { get; set; }   // Maps to "end_latlng"

        // Social and counts
        public int AchievementCount { get; set; }     // Maps to "achievement_count"
        public int KudosCount { get; set; }           // Maps to "kudos_count"
        public int CommentCount { get; set; }         // Maps to "comment_count"
        public int AthleteCount { get; set; }         // Maps to "athlete_count"
        public int PhotoCount { get; set; }           // Maps to "photo_count"
        public int TotalPhotoCount { get; set; }      // Maps to "total_photo_count"

        // Map data
        public string MapId { get; set; }             // Maps to "map.id"
        public string Polyline { get; set; }          // Maps to "map.polyline"
        public string SummaryPolyline { get; set; }   // Maps to "map.summary_polyline"

        // Activity flags
        public bool Trainer { get; set; }             // Maps to "trainer"
        public bool Commute { get; set; }             // Maps to "commute"
        public bool Manual { get; set; }              // Maps to "manual"
        public bool Private { get; set; }             // Maps to "private"
        public bool Flagged { get; set; }             // Maps to "flagged"
        public string Visibility { get; set; }        // Maps to "visibility"

        // Gear and device
        public string GearId { get; set; }            // Maps to "gear_id"
        public string DeviceName { get; set; }        // Maps to "device_name"
        public string EmbedToken { get; set; }        // Maps to "embed_token"

        // Performance metrics
        public double AverageSpeed { get; set; }      // Maps to "average_speed"
        public double MaxSpeed { get; set; }          // Maps to "max_speed"
        public double? AverageCadence { get; set; }   // Maps to "average_cadence"
        public double? AverageWatts { get; set; }     // Maps to "average_watts"
        public int? WeightedAverageWatts { get; set; }// Maps to "weighted_average_watts"
        public double? Kilojoules { get; set; }       // Maps to "kilojoules"
        public bool? DeviceWatts { get; set; }        // Maps to "device_watts"
        public int? MaxWatts { get; set; }            // Maps to "max_watts"
        public double? ElevHigh { get; set; }         // Maps to "elev_high"
        public double? ElevLow { get; set; }          // Maps to "elev_low"
        public int PrCount { get; set; }              // Maps to "pr_count"
        public double? Calories { get; set; }         // Maps to "calories"

        // Additional metadata
        public bool HasKudoed { get; set; }           // Maps to "has_kudoed"
        public int? WorkoutType { get; set; }         // Maps to "workout_type"
        public string Description { get; set; }       // Maps to "description"
        public int ResourceState { get; set; }        // Maps to "resource_state"

        // Complex objects as JSON strings (or separate tables if preferred)
        public string SegmentEffortsJson { get; set; } // Maps to "segment_efforts" as JSON
        public string SplitsMetricJson { get; set; }   // Maps to "splits_metric" as JSON
        public string LapsJson { get; set; }           // Maps to "laps" as JSON
    }
}
