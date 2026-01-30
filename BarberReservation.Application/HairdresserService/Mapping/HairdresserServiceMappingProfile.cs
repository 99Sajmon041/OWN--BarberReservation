using AutoMapper;
using BarberReservation.Application.HairdresserService.Commands.Self.CreateSelfHairdresserService;
using BarberReservation.Shared.Models.HairdresserService;

namespace BarberReservation.Application.HairdresserService.Mapping;

public sealed class HairdresserServiceMappingProfile : Profile
{
    public HairdresserServiceMappingProfile()
    {
        CreateMap<BarberReservation.Domain.Entities.HairdresserService, HairdresserServiceDto>()
            .ForMember(x => x.HairdresserName, opt => opt.MapFrom(x => x.Hairdresser.FullName))
            .ForMember(x => x.ServiceName, opt => opt.MapFrom(x => x.Service.Name));

        CreateMap<CreateSelfHairdresserServiceCommand, BarberReservation.Domain.Entities.HairdresserService>()
            .ForMember(x => x.IsActive, opt => opt.Ignore())
            .AfterMap((y, x) => x.IsActive = true);
    }
}
