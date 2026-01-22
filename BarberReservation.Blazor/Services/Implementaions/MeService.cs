using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Models.User.Common;

namespace BarberReservation.Blazor.Services.Implementaions;

public sealed class MeService(IApiClient api) : IMeService
{
    public async Task<UserDto> GetProfile(CancellationToken ct)
    {
        return await api.GetAsync<UserDto>("api/users/me", ct);
    }
}
