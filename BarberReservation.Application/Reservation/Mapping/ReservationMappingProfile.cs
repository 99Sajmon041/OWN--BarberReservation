using AutoMapper;
using BarberReservation.Shared.Models.Rezervation.Admin;

namespace BarberReservation.Application.Reservation.Mapping;

public sealed class ReservationMappingProfile : Profile
{
    public ReservationMappingProfile()
    {
        CreateMap<BarberReservation.Domain.Entities.Reservation, AdminReservationDto>()
            .ForMember(x => x.HairdresserFullName, opt => opt.MapFrom(x => $"{x.Hairdresser.FirstName} {x.Hairdresser.LastName}"))
            .ForMember(x => x.ServiceName, opt => opt.MapFrom(x => x.HairdresserService.Service.Name))
            .ForMember(x => x.DurationMinutes, opt => opt.MapFrom(x => x.HairdresserService.DurationMinutes))
            .ForMember(x => x.Price, opt => opt.MapFrom(x => x.HairdresserService.Price))
            .ForMember(x => x.ClientFullName, opt => opt.MapFrom(x => x.CustomerName))
            .ForMember(x => x.ClientEmail, opt => opt.MapFrom(x => x.CustomerEmail))
            .ForMember(x => x.ClientPhone, opt => opt.MapFrom(x => x.CustomerPhone));
    }
}
