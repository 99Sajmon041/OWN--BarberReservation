using AutoMapper;
using BarberReservation.Domain.Entities;
using BarberReservation.Application.Authorization.Command.Register;
using BarberReservation.Shared.Models.User.Common;

namespace BarberReservation.Application.User.Mapping;

public sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterCommand, ApplicationUser>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));

        CreateMap<ApplicationUser, UserDto>();
    }
}
