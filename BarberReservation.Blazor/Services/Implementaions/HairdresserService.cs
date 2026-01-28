using BarberReservation.Blazor.Auth;
using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService;

namespace BarberReservation.Blazor.Services.Implementaions;

public sealed class HairdresserService(IApiClient api, AuthState authState) : IHairdresserService
{
    public async Task<PagedResult<HairdresserServiceDto>> GetAllAsync(CommonHairdresserServicePagedRequest request, CancellationToken ct)
    {
        await authState.LoadAsync();

        var parts = new List<string>()
        {
            $"page={request.Page}",
            $"pageSize={request.PageSize}"
        };

        if (!string.IsNullOrWhiteSpace(request.Search))
            parts.Add($"search={Uri.EscapeDataString(request.Search.Trim())}");

        if (!string.IsNullOrWhiteSpace(request.SortBy))
            parts.Add($"sortBy={Uri.EscapeDataString(request.SortBy.Trim())}");

        if (request.Desc)
            parts.Add("desc=true");

        if (request.ServiceId is not null)
            parts.Add($"serviceId={request.ServiceId}");

        if (request.IsActive is not null)
            parts.Add($"isActive={(request.IsActive.Value ? "true" : "false")}");

        if (authState.Roles.Contains(nameof(UserRoles.Admin)))
        {
            if (!string.IsNullOrWhiteSpace(request.HairdresserId))
                parts.Add($"hairdresserId={Uri.EscapeDataString(request.HairdresserId.Trim())}");
        }

        var qs = string.Join("&", parts);
        var url = string.Empty;

        if(authState.Roles.Contains(nameof(UserRoles.Admin)))
        {
            url = $"api/admin/hairdresser-services?{qs}";
        }
        else
        {
            url = $"api/me/hairdresser-services?{qs}";
        }

        return await api.GetAsync<PagedResult<HairdresserServiceDto>>(url, ct);
    }
}
