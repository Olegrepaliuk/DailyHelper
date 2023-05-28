using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeService.Models;

namespace HelperTelegramBot.Http
{
    public interface ITimeDataClient
    {
        Task<DayInfoResponse> GetDayInfo(DateTime day);
        Task<ConvertTimeResponse> ConvertTime(string unitFrom, string unitTo, double value);
    }
}
