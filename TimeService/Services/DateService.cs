using System.Globalization;
using TimeService.Models;

namespace TimeService.Services
{
    public class DateService : IDateService
    {

        public YearInfo GetYearInfo(int year)
        {
            if ((year < 1) || (year > 9999))
            {
                throw new ArgumentOutOfRangeException("Invalid year value");
            }

            bool isLeapYear = DateTime.IsLeapYear(year);
            
            DateOnly dayOfYear = new DateOnly(year, 1, 1);
            DateOnly startDay = dayOfYear;
            DateOnly endDay = new DateOnly(year, 12, 31);

            int[] weekDaysCounter = new int[7]; //declare an array to store total number for every day

            int calendarWeeks = 0;
            
            while (dayOfYear.Year == year)
            {
                int dayNum = (int)dayOfYear.DayOfWeek;

                weekDaysCounter[dayNum]++;
                               
                if(dayOfYear.DayOfWeek == DayOfWeek.Sunday)
                {
                    calendarWeeks++;                   
                }

                if(dayOfYear != DateOnly.MaxValue)
                {
                    dayOfYear = dayOfYear.AddDays(1);
                }
                else
                {
                    break;
                }
                
            }

            YearInfo yearInfo = new YearInfo
            {
                Year = year,
                IsLeap = isLeapYear,
                DaysNumber = isLeapYear ? 366 : 365,
                CalendarWeeksNumber = calendarWeeks,
                FullWeeksNumber = Math.Min(weekDaysCounter[1], weekDaysCounter[0]),
                MondaysNum = weekDaysCounter[1],
                TuesdaysNum = weekDaysCounter[2],
                WednesdaysNum = weekDaysCounter[3],
                ThursdaysNum = weekDaysCounter[4],
                FridaysNum = weekDaysCounter[5],
                SaturdaysNum = weekDaysCounter[6],
                SundaysNum = weekDaysCounter[0],
                StartDay = startDay.DayOfWeek.ToString(),
                EndDay = endDay.DayOfWeek.ToString()
            };

            return yearInfo;
            

        }

        public int DatesSusbtractDays(DatesSubstractRequest datesSubstract)
        {
            int daysNumber = 0;

            DateTime firstDate = datesSubstract.FirstDate;
            DateTime secondDate = datesSubstract.SecondDate;

            if (firstDate > secondDate)
            {
                daysNumber = (firstDate - secondDate).Days;
            }
            else
            {
                daysNumber = (secondDate - firstDate).Days;
            }
            
            return daysNumber;
        }

        public DayInfo GetDayInfo(DateTime day)
        {
            int totalDaysInYear = DateTime.IsLeapYear(day.Year) ? 366 : 365;
            int dayNumberInYear = day.DayOfYear;
            int weekNumber = GetWeekNumber(day);

            int yearCompletedPercent = Convert.ToInt32(Math.Round(((double)dayNumberInYear / totalDaysInYear) * 100, 0, MidpointRounding.AwayFromZero));

            DayInfo dayInfo = new DayInfo
            {
                Date = day,
                DayNumberInYear = dayNumberInYear,
                WeekDayName = day.DayOfWeek.ToString(),
                DaysToNewYear = totalDaysInYear - dayNumberInYear,
                YearCompletedPercent = yearCompletedPercent,
                WeekNumber = weekNumber
            };

            return dayInfo;
        }

        private DateTime AddTime(DateTime startDate, string unit, int value, bool isAdd)
        {

            DateTime result = startDate;

            int addOrSub = isAdd == true ? 1 : -1;
            TimeUnit timeUnit = GetTimeUnit(unit);

            switch (timeUnit)
            {
                case TimeUnit.HOUR:
                    result = startDate.AddHours(addOrSub * Convert.ToDouble(value));
                    break;
                case TimeUnit.DAY:
                    result = startDate.AddDays(addOrSub * Convert.ToDouble(value));
                    break;
                case TimeUnit.WEEK:
                    result = startDate.AddDays(addOrSub * 7 * Convert.ToDouble(value));
                    break;
                case TimeUnit.MONTH:
                    result = startDate.AddMonths(addOrSub * value);
                    break;
                default:
                    throw new Exception("This time unit is not supported");
                    break;
            }

            return result;
        }

        public double ConvertTime(string convertUnitFrom, string convertUnitTo, double convertValue)
        {
            double result = 0;

            double convertMultiplier = 1;

            TimeUnit unitFrom = GetTimeUnit(convertUnitFrom);
            TimeUnit unitTo = GetTimeUnit(convertUnitTo);

            if (unitFrom == unitTo)
            {
                return convertValue;
            }

            bool convertToSmaller = unitFrom > unitTo;
            int convertStep = convertToSmaller ? -1 : 1;

            List<Tuple<TimeUnit, TimeUnit, int>> convertMultipliers = new List<Tuple<TimeUnit, TimeUnit, int>>
            {
                new Tuple<TimeUnit, TimeUnit, int>(TimeUnit.SECOND, TimeUnit.MINUTE, 60),
                new Tuple<TimeUnit, TimeUnit, int>(TimeUnit.MINUTE, TimeUnit.HOUR, 60),
                new Tuple<TimeUnit, TimeUnit, int>(TimeUnit.HOUR, TimeUnit.DAY, 24),
                new Tuple<TimeUnit, TimeUnit, int>(TimeUnit.DAY, TimeUnit.WEEK, 7)
            };

            int i = (int)unitFrom;

            while (i != (int)unitTo)
            {
                TimeUnit stepFromUnit = (TimeUnit) i;
                TimeUnit stepToUnit = (TimeUnit) i + convertStep;



                Tuple<TimeUnit, TimeUnit, int>? convertStepMultiplier = convertMultipliers
                                                                    .FirstOrDefault(cm=>cm.Item1 == stepFromUnit && cm.Item2 == stepToUnit
                                                                    || cm.Item2 == stepFromUnit && cm.Item1 == stepToUnit);


                if(convertStepMultiplier == null)
                {
                    throw new Exception("Unable to convert value");
                }

                convertMultiplier *= convertStepMultiplier.Item3;

                i += convertStep;
               
            }

            if (convertToSmaller)
            {
                result = convertValue * convertMultiplier;
            }
            else
            {
                result = Math.Round(convertValue / convertMultiplier, 2, MidpointRounding.AwayFromZero);
            }

            return result;
        }

        private TimeUnit GetTimeUnit (string timeUnit)
        {
            TimeUnit timeUnitEnum;

            try
            {
                timeUnitEnum = (TimeUnit)Enum.Parse(typeof(TimeUnit), timeUnit);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Invalid time unit value");
            }

            return timeUnitEnum;
        }

        private int GetWeekNumber(DateTime day)
        {
            int weekNumber;

            DateTime firstDayOfYear = new DateTime(day.Year, 1, 1);

            DateTime firstDayFirstWeek;

            firstDayFirstWeek = firstDayOfYear;

            if (firstDayOfYear.DayOfWeek != DayOfWeek.Monday)
            {               
                while (firstDayFirstWeek.DayOfWeek != DayOfWeek.Monday)
                {
                    firstDayFirstWeek = firstDayFirstWeek.AddDays(1);
                }
            }
            
            if (firstDayFirstWeek > day)
            {
                weekNumber = 52; //last week of last year
            }
            else
            {
                const int totalDaysInWeek = 7;

                weekNumber = (day - firstDayFirstWeek).Days / totalDaysInWeek + 1;
            }

            return weekNumber;
        }

        public DateTime PerformTimeOperation(DateTime startDate, string timeUnit, int value, string operation)
        {
            string[] availableOperations = { "+", "-" };

            if (!availableOperations.Contains(operation))
            {
                throw new ArgumentException("Operation is not supported");
            }

            bool isAdd = operation == "+" ? true : false;

            return AddTime(startDate, timeUnit, value, isAdd);
        }
    }
}
