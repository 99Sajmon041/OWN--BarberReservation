using BarberReservation.Shared.Models.User.Common;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface IMeService
{
    Task<UserDto> GetProfile(CancellationToken ct);
}
