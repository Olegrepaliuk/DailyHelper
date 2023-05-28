using TimeService.Models;

namespace TimeService.Services
{
    public interface IDateService
    {
        YearInfo GetYearInfo(int year);
        int DatesSusbtractDays(DatesSubstractRequest datesSubstract);
        DayInfo GetDayInfo(DateTime day);
        double ConvertTime(string convertUnitFrom, string convertUnitTo, double convertValue);
        DateTime PerformTimeOperation(DateTime startDate, string timeUnit, int value, string operation);
    }
}
