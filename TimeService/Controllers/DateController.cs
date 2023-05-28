using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using TimeService.Models;
using TimeService.Services;

namespace TimeService.Controllers
{
    [Route("api/d")]
    public class DateController : Controller
    {
        private readonly IDateService _dateService;
        private readonly IMapper _mapper;

        public DateController(IDateService dateService, IMapper mapper)
        {
            _dateService = dateService;
            _mapper = mapper;
        }


        [HttpGet("year/{year}")]
        public async Task<IActionResult> GetYearInfo(int year)
        {
            YearInfo yearInfo = _dateService.GetYearInfo(year);

            return Ok(_mapper.Map<YearInfoResponse>(yearInfo));
        }

        [HttpPost("substract")]
        public async Task<IActionResult> SubstractDates([FromBody] DatesSubstractRequest datesSubstractRequest)
        {
            int daysNumber = _dateService.DatesSusbtractDays(datesSubstractRequest);

            DatesSubstractResponse datesSubstractResponse = new DatesSubstractResponse()
            {
                FirstDate = datesSubstractRequest.FirstDate,
                SecondDate = datesSubstractRequest.SecondDate,
                DaysPassed = daysNumber
            };

            return Ok(datesSubstractResponse);
        }

        [HttpPost("time/operation")]
        public async Task<IActionResult> PerformTimeOperation([FromBody] TimeOperationRequest timeopRequest)
        {
            DateTime startDate = timeopRequest.StartDate;
            string timeUnit = timeopRequest.TimeUnit;
            int value = timeopRequest.Value;
            string operation = timeopRequest.Operation;

            DateTime result = _dateService.PerformTimeOperation(startDate, timeUnit, value, operation);

            TimeOperationResponse timeOperationResponse = new TimeOperationResponse()
            {
                StartDate = startDate,
                TimeUnit = timeUnit,
                Value = value,
                Operation = operation,
                Result = result
            };

            return Ok(timeOperationResponse);
        }

        [HttpPost("info")]
        public async Task<IActionResult> GetDayInfo([FromBody] DayInfoRequest dayInfoRequest)
        {
            DayInfo dayInfo = _dateService.GetDayInfo(dayInfoRequest.Day);

            return Ok(_mapper.Map<DayInfoResponse>(dayInfo));
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertTimeUnit([FromBody] ConvertTimeRequest convertTimeRequest)
        {
            string convertUnitFrom = convertTimeRequest.UnitFrom;
            string convertUnitTo = convertTimeRequest.UnitTo;
            double convertValue = convertTimeRequest.Value;

            double result = _dateService.ConvertTime(convertUnitFrom, convertUnitTo, convertValue);

            ConvertTimeResponse convertTimeResponse = new ConvertTimeResponse
            {
                UnitFrom = convertUnitFrom,
                UnitTo = convertUnitTo,
                UnitFromValue = convertValue,
                Result = result
            };

            return Ok(convertTimeResponse);
        }
    }
}
