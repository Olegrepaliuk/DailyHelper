using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TimeService.Models
{
    public class DatesSubstractResponse
    {
        [JsonPropertyName("firstDate")]
        public DateTime FirstDate { get; set; }

        [JsonPropertyName("secondDate")]
        public DateTime SecondDate { get; set; }

        [JsonPropertyName("daysPassed")]
        public int DaysPassed { get; set; }
    }
}
