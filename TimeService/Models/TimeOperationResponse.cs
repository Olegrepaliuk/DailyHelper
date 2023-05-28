using System.Text.Json.Serialization;

namespace TimeService.Models
{
    public class TimeOperationResponse
    {
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("timeUnit")]
        public string TimeUnit { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }

        [JsonPropertyName("operation")]
        public string Operation { get; set; }

        [JsonPropertyName("result")]
        public DateTime Result { get; set; }
    }
}
