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

        return await api.GetAsync<List<HairdresserTimeOffDto>>($"api/me/time-off/daily?{qs}", ct);
    }

    public async Task CreateAsync(UpsertTimeOffRequest request, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Post, "api/me/time-off", request, ct);
    }
    public async Task UpdateAsync(int id, UpsertTimeOffRequest request, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Put, $"api/me/time-off/{id}", request, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Delete, $"api/me/time-off/{id}", null, ct);
    }

    public async Task<PagedResult<HairdresserTimeOffDto>> GetAllAsync(HairdresserTimeOffPagedRequest request, CancellationToken ct)
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

        var url = isAdmin ? $"api/admin/time-off?{qs}" : $"api/me/time-off?{qs}";

        return await api.GetAsync<PagedResult<HairdresserTimeOffDto>>(url, ct);
    }

    public async Task<List<HairdresserTimeOffDto>> GetAllWeeklyAsync(string hairdresserId, DateTime weekStartDate, CancellationToken ct)
    {
        await authState.LoadAsync();

        string url = string.Empty;

        if (authState.Roles.Contains(nameof(UserRoles.Admin)))
            url = "api/admin/time-off/weekly";
        else if (authState.Roles.Contains(nameof(UserRoles.Hairdresser)))
            url = "api/me/time-off/weekly";
        else
            throw new ApiRequestException("Nelze zobrazit kalendnář - nemáte právo.", StatusCodes.Status403Forbidden);

        string parameters;

        if (url.StartsWith("api/admin"))
            parameters = $"hairdresserId={Uri.EscapeDataString(hairdresserId.Trim())}&weekStartDate={Uri.EscapeDataString(weekStartDate.ToString("yyyy-MM-dd"))}";
        else
            parameters = $"weekStartDate={Uri.EscapeDataString(weekStartDate.ToString("yyyy-MM-dd"))}";

        return await api.GetAsync<List<HairdresserTimeOffDto>>($"{url}?{parameters}", ct);
    }
}
