using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Shared.Models.User.Common;
using MediatR;

namespace BarberReservation.Application.User.Queries.Self.GetProfile;

public sealed class GetProfileQueryHandler(
    ICurrentAppUser currentAppUser,
    IMapper mapper) : IRequestHandler<GetProfileQuery, UserDto>
{
    public Task<UserDto> Handle(GetProfileQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var appUser = currentAppUser.User ?? throw new UnauthorizedException("Uživatel není přihlášen.");

        return Task.FromResult(mapper.Map<UserDto>(appUser));
    }
}
