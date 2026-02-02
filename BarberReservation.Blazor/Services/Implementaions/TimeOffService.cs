using BarberReservation.Blazor.Auth;
using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.TimeOff;
using System;

namespace BarberReservation.Blazor.Services.Implementaions;

public sealed class TimeOffService(IApiClient api, AuthState authState) : ITimeOffService
{
    public async Task<List<HairdresserTimeOffDto>> GetByDayAsync(DateOnly day, CancellationToken ct)
    {
        var qs = $"day={day:yyyy-MM-dd}";

        return await api.GetAsync<List<HairdresserTimeOffDto>>($"api/me/timeoff/daily?{qs}", ct);
    }

    public async Task CreateAsync(UpsertTimeOffRequest request, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Post, "api/me/timeoff", request, ct);
    }
    public async Task UpdateAsync(int id, UpsertTimeOffRequest request, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Put, $"api/me/timeoff/{id}", request, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Delete, $"api/me/timeoff/{id}", null, ct);
    }

    public async Task<PagedResult<HairdresserTimeOffDto>> GetAllAsync(HairdresserPagedRequest request, CancellationToken ct)
    {
        await authState.LoadAsync();
        var isAdmin = authState.Roles.Contains(nameof(UserRoles.Admin));

        var parts = new List<string>
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

        if (isAdmin && !string.IsNullOrWhiteSpace(request.HairdresserId))
            parts.Add($"hairdresserId={Uri.EscapeDataString(request.HairdresserId.Trim())}");

        if (request.Month is not null)
            parts.Add($"month={request.Month}");

        if (request.Year is not null)
            parts.Add($"year={request.Year}");

        var qs = string.Join("&", parts);

        var url = isAdmin ? $"api/admin/time-off?{qs}" : $"api/me/timeoff?{qs}";

        return await api.GetAsync<PagedResult<HairdresserTimeOffDto>>(url, ct);
    }
}
