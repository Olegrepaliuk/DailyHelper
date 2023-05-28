using System.Text.Json.Serialization;

namespace TimeService.Models
{
    public class YearInfoResponse
    {
        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("isLeap")]
        public bool IsLeap { get; set; }

        [JsonPropertyName("daysNumber")]
        public int DaysNumber { get; set; }

        [JsonPropertyName("mondaysNum")]
        public int MondaysNum { get; set; }

        [JsonPropertyName("tuesdaysNum")]
        public int TuesdaysNum { get; set; }

        [JsonPropertyName("wednesdaysNum")]
        public int WednesdaysNum { get; set; }

        [JsonPropertyName("thursdaysNum")]
        public int ThursdaysNum { get; set; }

        [JsonPropertyName("fridaysNum")]
        public int FridaysNum { get; set; }

        [JsonPropertyName("saturdaysNum")]
        public int SaturdaysNum { get; set; }

        [JsonPropertyName("sundaysNum")]
        public int SundaysNum { get; set; }

        [JsonPropertyName("startDay")]
        public string StartDay { get; set; }

        [JsonPropertyName("endDay")]
        public string EndDay { get; set; }
    }
}
