namespace Strava2ExcelWebApiBackend.Helpers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class SegmentEffortsConverter : JsonConverter<string>
    {
        public override string ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Deserialize the array into a JSON string
            var jsonArray = JArray.Load(reader);
            return jsonArray.ToString(Formatting.None);  // Return as a JSON string
        }

        public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
        {
            // Write the string value back as JSON
            writer.WriteRawValue(value ?? "[]");
        }
    }

}
