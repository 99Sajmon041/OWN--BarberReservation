using BarberReservation.Shared.Models.User.Common;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface IMeService
{
    Task<UserDto> GetProfileAsync(CancellationToken ct);
    Task DeactivateAccountAsync(CancellationToken ct);
    Task UpdateAccountAsync(UpdateUserRequest request, CancellationToken ct);
}
