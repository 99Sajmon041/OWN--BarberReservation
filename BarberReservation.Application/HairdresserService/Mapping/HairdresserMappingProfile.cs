using AutoMapper;
using BarberReservation.Application.HairdresserService.Commands.Self.CreateSelfHairdresserService;
using BarberReservation.Shared.Models.HairdresserService;

namespace BarberReservation.Application.HairdresserService.Mapping;

public sealed class HairdresserMappingProfile : Profile
{
    public HairdresserMappingProfile()
    {
        CreateMap<BarberReservation.Domain.Entities.HairdresserService, HairdresserServiceDto>()
            .ForMember(x => x.HairdresserName, opt => opt.MapFrom(x => x.Hairdresser.FullName))
            .ForMember(x => x.ServiceName, opt => opt.MapFrom(x => x.Service.Name));

        CreateMap<CreateSelfHairdresserServiceCommand, BarberReservation.Domain.Entities.HairdresserService>();
    }
}
