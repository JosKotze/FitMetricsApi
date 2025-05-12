using Newtonsoft.Json.Linq;
using Strava2ExcelWebApiBackend.Helpers;
using Strava2ExcelWebApiBackend.Models;
using System.Text.Json.Serialization;

namespace Strava2ExcelWebApiBackend.Models
{
    public class StravaActivityData
    {
        public string? pace { get; set; }
        public int resource_state { get; set; }
        public User? Athlete { get; set; }
        public string? name { get; set; }
        public double distance { get; set; }
        public int moving_time { get; set; }
        public int elapsed_time { get; set; }
        public double total_elevation_gain { get; set; }
        public string? type { get; set; }
        public string? sport_type { get; set; }
        public int? workout_type { get; set; }
        public long id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? start_date_local { get; set; }
        public string? timezone { get; set; }
        public double? utc_offset { get; set; }
        public string? location_city { get; set; }
        public string? location_state { get; set; }
        public string? location_country { get; set; }
        public int? achievement_count { get; set; }
        public int? kudos_count { get; set; }
        public int? comment_count { get; set; }
        public int? athlete_count { get; set; }
        public int? photo_count { get; set; }
        public Map? map { get; set; }
        public bool? trainer { get; set; }
        public bool? commute { get; set; }
        public bool? manual { get; set; }
        public bool? Private { get; set; }
        public string? visibility { get; set; }
        public bool? flagged { get; set; }
        public string? gear_id { get; set; }
        public List<double>? start_latlng { get; set; }
        public List<double>? end_latlng { get; set; }
        public double average_speed { get; set; }
        public double max_speed { get; set; }
        public double? average_cadence { get; set; }
        public double? average_watts { get; set; }
        public double? max_watts { get; set; }
        public double? weighted_average_watts { get; set; }
        public double? kilojoules { get; set; }
        public bool? device_watts { get; set; }
        public bool has_heartrate { get; set; }
        public double? average_heartrate { get; set; }
        public double? max_heartrate { get; set; }
        public double? elev_high { get; set; }
        public double? elev_low { get; set; }
        public int pr_count { get; set; }
        public long? upload_id { get; set; }
        public string? external_id { get; set; }
        public int total_photo_count { get; set; }
        public bool has_kudoed { get; set; }
        public int user_id { get; set; }

        public string? description { get; set; }
        public double calories { get; set; }
        public string? device_name { get; set; }
        public string? embed_token { get; set; }

        public List<SegmentEffort>? segment_efforts { get; set; }
        public List<SplitsMetric>? splits_metric { get; set; }

        public List<Lap> laps { get; set; }

    }
}

public class Lap
{
    public long id { get; set; }
    public int resource_state { get; set; }
    public string name { get; set; }
    public double distance { get; set; }
    public int elapsed_time { get; set; }
    public int moving_time { get; set; }
    public DateTime? start_date { get; set; }
    public DateTime? start_date_local { get; set; }
    public double average_speed { get; set; }
    public double max_speed { get; set; }
    public int lap_index { get; set; }
    public int split { get; set; }
    public int start_index { get; set; }
    public int end_index { get; set; }
    public double total_elevation_gain { get; set; }
    public double? average_cadence { get; set; }
    public bool? device_watts { get; set; }
}

public class SegmentEffort
{
    long Id { get; set; }
    public int ResourceState { get; set; }
    public string Name { get; set; }
    public Activity Activity { get; set; }  // Related Activity
    public User Athlete { get; set; }    // Related Athlete
    public int ElapsedTime { get; set; }
    public int MovingTime { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime StartDateLocal { get; set; }
    public double Distance { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public bool DeviceWatts { get; set; }
    public double AverageHeartRate { get; set; }
    public double MaxHeartRate { get; set; }
    public Segment Segment { get; set; }  // Related Segment
    public int PrRank { get; set; }
    public List<Achievement> Achievements { get; set; }  // Related Achievements
    public string Visibility { get; set; }
    public bool Hidden { get; set; }
}

public class Segment
{
    public long Id { get; set; }
    public int ResourceState { get; set; }
    public string Name { get; set; }
    public string ActivityType { get; set; }
    public double Distance { get; set; }
    public double AverageGrade { get; set; }
    public double MaximumGrade { get; set; }
    public double ElevationHigh { get; set; }
    public double ElevationLow { get; set; }
    public List<double> StartLatlng { get; set; }
    public List<double> EndLatlng { get; set; }
    public int ClimbCategory { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public bool Private { get; set; }
    public bool Hazardous { get; set; }
    public bool Starred { get; set; }
}

public class Achievement
{
    public int TypeId { get; set; }
    public string Type { get; set; }
    public int Rank { get; set; }
}


public class SplitsMetric
{
    public double Time { get; set; }
    public double Pace { get; set; }
    // Define other relevant properties based on the structure of your data
}

