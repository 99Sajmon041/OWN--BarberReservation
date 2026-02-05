using AutoMapper;
using BarberReservation.Shared.Models.Reservation.Common;

namespace BarberReservation.Application.Reservation.Mapping;

public sealed class ReservationMappingProfile : Profile
{
    public ReservationMappingProfile()
    {
        CreateMap<BarberReservation.Domain.Entities.Reservation, ReservationDto>()
            .ForMember(x => x.HairdresserFullName, opt => opt.MapFrom(x => x.Hairdresser != null ?  $"{x.Hairdresser.FirstName} {x.Hairdresser.LastName}" : null))
            .ForMember(x => x.ServiceName, opt => opt.MapFrom(x => x.HairdresserService.Service.Name))
            .ForMember(x => x.DurationMinutes, opt => opt.MapFrom(s => s.HairdresserService != null ? s.HairdresserService.DurationMinutes : 0))
            .ForMember(x => x.Price, opt => opt.MapFrom(s => s.HairdresserService != null ? s.HairdresserService.Price : 0m))
            .ForMember(x => x.ClientFullName, opt => opt.MapFrom(x => x.Customer != null ? x.CustomerName : null))
            .ForMember(x => x.ClientEmail, opt => opt.MapFrom(x => x.Customer != null ?  x.CustomerEmail : null))
            .ForMember(x => x.ClientPhone, opt => opt.MapFrom(x => x.Customer != null ?  x.CustomerPhone : null))
            .ForMember(x => x.CustomerId, opt => opt.MapFrom(x => x.Customer != null ?  x.CustomerId : null));
    }
}
