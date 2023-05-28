using AutoMapper;
using TimeService.Models;

namespace TimeService.Profiles
{
    public class DateProfile : Profile
    {
        public DateProfile()
        {
            CreateMap<YearInfo, YearInfoResponse>();
            CreateMap<DayInfo, DayInfoResponse>();
        }

    }
}
