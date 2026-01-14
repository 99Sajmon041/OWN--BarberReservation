using AutoMapper;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Admin;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;

namespace BarberReservation.Application.HairdresserWorkingHours.Mapping;

public sealed class HairdresserWorkingHoursMappingProfile : Profile
{
    public HairdresserWorkingHoursMappingProfile()
    {
        CreateMap<BarberReservation.Domain.Entities.HairdresserWorkingHours, AdminHairdresserWorkingHoursDto>()
            .ForMember(x => x.HairdresserName, opt => opt.MapFrom(x => $"{x.Hairdresser.FirstName} {x.Hairdresser.LastName}"));

        CreateMap<BarberReservation.Domain.Entities.HairdresserWorkingHours, HairdresserWorkingHoursDto>();

        CreateMap<HairdresserWorkingHoursUpsertDto, BarberReservation.Domain.Entities.HairdresserWorkingHours>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.HairdresserId, opt => opt.Ignore())
            .ForMember(x => x.DayOfWeek, opt => opt.Ignore());
    }
}
