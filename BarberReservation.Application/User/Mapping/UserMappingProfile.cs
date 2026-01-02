using AutoMapper;
using BarberReservation.Application.Authorization.Command.Register;
using BarberReservation.Application.User.Commands.Admin.CreateUser;
using BarberReservation.Application.User.Commands.Admin.PartlyUpdateUser;
using BarberReservation.Application.User.Commands.Self.UpdateAccount;
using BarberReservation.Domain.Entities;
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
            .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.FirstName))
            .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.LastName))
            .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(x => x.PhoneNumber))
            .ForAllMembers(x => x.Ignore());

        CreateMap<CreateUserCommand, ApplicationUser>();

        CreateMap<PartlyUpdateUserCommand, ApplicationUser>()
            .ForMember(x => x.Id, opt => opt.Ignore());
    }
}