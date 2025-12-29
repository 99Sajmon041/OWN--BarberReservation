using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Models.User.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BarberReservation.Application.User.Queries.Self.GetProfile;

public sealed class GetProfileQueryHandler(
    UserManager<ApplicationUser> userManager,
    IUserContext userContext,
    IMapper mapper) : IRequestHandler<GetProfileQuery, UserDto>
{
    public async Task<UserDto> Handle(GetProfileQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var current = userContext.GetCurrentUser() ?? throw new UnauthorizedException("Uživatel nebyl rozpoznán.");

        var appUser = await userManager.FindByIdAsync(current.Id) ?? throw new UnauthorizedException("Uživatel nebyl nalezen.");

        return mapper.Map<UserDto>(appUser);
    }
}
