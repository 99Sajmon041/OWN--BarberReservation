using AutoMapper;
using BarberReservation.Application.Authorization.Command.Register;
using BarberReservation.Application.User.Commands.Admin.CreateHairdresser;
using BarberReservation.Application.User.Commands.Admin.PartlyUpdateUser;
using BarberReservation.Application.User.Commands.Self.UpdateAccount;
using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Models.LookUpModels;
using BarberReservation.Shared.Models.User.Common;

namespace BarberReservation.Application.User.Mapping;

public sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterCommand, ApplicationUser>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));

        CreateMap<ApplicationUser, UserDto>();

        CreateMap<UpdateAccountCommand, ApplicationUser>()
            .ForMember(x => x.Id, opt => opt.Ignore());

        CreateMap<CreateHairdresserCommand, ApplicationUser>();

        CreateMap<PartlyUpdateUserCommand, ApplicationUser>()
            .ForMember(x => x.Id, opt => opt.Ignore());

        CreateMap<ApplicationUser, ReservationClientLookUpDto>()
            .ForMember(x => x.CustomerId, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.CustomerName, opt => opt.MapFrom(x => $"{x.FirstName} {x.LastName}"))
            .ForMember(x => x.CustomerEmail, opt => opt.MapFrom(x => x.Email))
            .ForMember(x => x.CustomerPhone, opt => opt.MapFrom(x => x.PhoneNumber));

        CreateMap<ApplicationUser, GetLookUpHairdressersByService>()
            .ForMember(x => x.FullName, opt => opt.MapFrom(x => $"{x.FirstName} {x.LastName}"));
    }
}