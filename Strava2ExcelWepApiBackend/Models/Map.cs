namespace Strava2ExcelWebApiBackend.Models
{
    public class Map
    {
        public int Id { get; set; }
        public long ActivityId { get; set; }
        public int UserId { get; set; }
        public string Polyline { get; set; }
    }
}
