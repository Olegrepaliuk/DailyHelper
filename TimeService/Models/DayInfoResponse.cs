using System.Text.Json.Serialization;

namespace TimeService.Models
{
    public class DayInfoResponse
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("weekDayName")]
        public string WeekDayName { get; set; }

        [JsonPropertyName("dayNumberInYear")]
        public int DayNumberInYear { get; set; }

        [JsonPropertyName("daysToNewYear")]
        public int DaysToNewYear { get; set; }

        [JsonPropertyName("yearCompletedPercent")]
        public int YearCompletedPercent { get; set; }

        [JsonPropertyName("weekNumber")]
        public int WeekNumber { get; set; }
    }
}
