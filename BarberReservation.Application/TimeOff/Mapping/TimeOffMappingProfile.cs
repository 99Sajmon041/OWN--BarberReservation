using AutoMapper;
using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Models.TimeOff.Admin;
using BarberReservation.Shared.Models.TimeOff.Hairdresser;

namespace BarberReservation.Application.TimeOff.Mapping;

public sealed class TimeOffMappingProfile : Profile
{
    public TimeOffMappingProfile()
    {
        CreateMap<HairdresserTimeOff, AdminHairdresserTimeOffDto>()
            .ForMember(x => x.HairdresserName, opt => opt.MapFrom(x => $"{x.Hairdresser.FirstName} {x.Hairdresser.LastName}"));

        CreateMap<HairdresserTimeOff, HairdresserTimeOffDto>();
    }
}
