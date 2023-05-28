using System.Text.Json.Serialization;

namespace TimeService.Models
{
    public class ConvertTimeResponse
    {
        [JsonPropertyName("unitFrom")]
        public string UnitFrom { get; set; }

        [JsonPropertyName("unitTo")]
        public string UnitTo { get; set; }

        [JsonPropertyName("unitFromValue")]
        public double UnitFromValue { get; set; }

        [JsonPropertyName("result")]
        public double Result { get; set; }
    }
}
