using System.Text.Json.Serialization;

namespace TimeService.Models
{
    public class ConvertTimeRequest
    {
        [JsonPropertyName("unitFrom")]
        public string UnitFrom { get; set; }

        [JsonPropertyName("unitTo")]
        public string UnitTo { get; set; }

        [JsonPropertyName("value")]
        public double Value { get; set; }
    }
}
