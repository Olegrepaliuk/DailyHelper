using System.Text.Json.Serialization;

namespace TimeService.Models
{
    public class TimeOperationRequest
    {
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("timeUnit")]
        public string TimeUnit { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }

        [JsonPropertyName("operation")]
        public string Operation { get; set; }
    }
}
