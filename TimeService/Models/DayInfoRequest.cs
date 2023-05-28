using System.Text.Json.Serialization;

namespace TimeService.Models
{
    public class DayInfoRequest
    {
        [JsonPropertyName("day")]
        public DateTime Day { get; set; }
    }
}
