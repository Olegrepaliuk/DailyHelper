using System.Text.Json.Serialization;

namespace TimeService.Models
{
    public class DayInfo
    {      
        public DateTime Date { get; set; }      
        public string WeekDayName { get; set; }
        public int DayNumberInYear { get; set; }
        public int DaysToNewYear { get; set; }     
        public int YearCompletedPercent { get; set; }      
        public int WeekNumber { get; set; }
    }
}
