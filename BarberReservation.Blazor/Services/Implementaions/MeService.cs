using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Models.User.Common;

namespace BarberReservation.Blazor.Services.Implementaions;

public sealed class MeService(IApiClient api) : IMeService
{
    public async Task<UserDto> GetProfileAsync(CancellationToken ct)
    {
        return await api.GetAsync<UserDto>("api/users/me", ct);
    }
    public async Task DeactivateAccountAsync(CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Patch, "api/users/me/deactivate", null, ct);
    }

    public async Task UpdateAccountAsync(UpdateUserRequest request, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Put, "api/users/me", request, ct);
    }
}
