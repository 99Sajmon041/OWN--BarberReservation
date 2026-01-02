using AutoMapper;
using BarberReservation.Application.Service.Commands.CreateService;
using BarberReservation.Application.Service.Commands.PartlyUpdateService;
using BarberReservation.Shared.Models.Service;

namespace BarberReservation.Application.Service.Mapping;

public sealed class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        CreateMap<BarberReservation.Domain.Entities.Service, ServiceDto>();

        CreateMap<PartlyUpdateServiceCommand, BarberReservation.Domain.Entities.Service>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.IsActive, opt => opt.Ignore());

        CreateMap<CreateServiceCommand, BarberReservation.Domain.Entities.Service>()
            .ForMember(x => x.Id, opt => opt.Ignore());
    }
}
