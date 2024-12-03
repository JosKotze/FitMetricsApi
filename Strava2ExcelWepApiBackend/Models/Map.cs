namespace Strava2ExcelWebApiBackend.Models
{
    public class Map
    {
        public string Id { get; set; }
        public string activityId { get; set; }
        public string SummaryPolyline { get; set; }
        public int ResourceState { get; set; }
    }
}
