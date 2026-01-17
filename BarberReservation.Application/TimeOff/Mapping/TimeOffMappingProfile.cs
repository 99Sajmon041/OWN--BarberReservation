using AutoMapper;
using BarberReservation.Application.TimeOff.Commands.Hairdresser.CreateSelfTimeOff;
using BarberReservation.Application.TimeOff.Commands.Hairdresser.UpdateSelfTimeOff;
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

        CreateMap<CreateSelfTimeOffCommand, HairdresserTimeOff>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => DateTime.UtcNow));
    }
}
