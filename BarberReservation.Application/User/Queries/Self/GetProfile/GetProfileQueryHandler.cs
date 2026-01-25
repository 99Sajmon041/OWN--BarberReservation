using AutoMapper;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Models.User.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BarberReservation.Application.User.Queries.Self.GetProfile;

public sealed class GetProfileQueryHandler(
    ICurrentAppUser currentAppUser,
    UserManager<ApplicationUser> userManager,
    IMapper mapper) : IRequestHandler<GetProfileQuery, UserDto>
{
    public async Task<UserDto> Handle(GetProfileQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var appUser = currentAppUser.User;
        var userDto = mapper.Map<UserDto>(appUser);
        userDto.Roles = (await userManager.GetRolesAsync(appUser)).ToList();

        return userDto;
    }
}
