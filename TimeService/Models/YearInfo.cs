using Microsoft.AspNetCore.Mvc;

namespace TimeService.Models
{
    public class YearInfo
    {
        public int Year { get; set; }
        public bool IsLeap { get; set; }
        public int DaysNumber { get; set; }
        public int CalendarWeeksNumber { get; set; }
        public int FullWeeksNumber { get; set; }
        public int MondaysNum { get; set; }
        public int TuesdaysNum { get; set; }
        public int WednesdaysNum { get; set; }
        public int ThursdaysNum { get; set; }
        public int FridaysNum { get; set; }
        public int SaturdaysNum { get; set; }
        public int SundaysNum { get; set; }
        public string StartDay { get; set; }
        public string EndDay { get; set; }

        public YearInfo()
        {

        }

        public YearInfo(int year)
        {
            Year = year;
        }
    }
}
