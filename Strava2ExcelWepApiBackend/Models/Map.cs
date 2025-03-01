using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Strava2ExcelWebApiBackend.Models
{
    public class Map
    {
        [Key]
        public long Id { get; set; }
        [JsonProperty("id")]  // Maps 'id' from the JSON to 'MapId' in your model
        public string? MapId { get; set; }
        public long ActivityId { get; set; }
        public int UserId { get; set; }
        public string? Polyline { get; set; }
        public List<double>? StartLatlng { get; set; }
        public List<double>? EndLatlng { get; set; }
    }
}
