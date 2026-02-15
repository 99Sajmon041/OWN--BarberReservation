using BarberReservation.Blazor.Auth;
using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.HairdresserWorkingHours;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;

namespace BarberReservation.Blazor.Services.Implementaions;

public sealed class WorkingHoursService(IApiClient api, AuthState authState) : IWorkingHoursService
{
    public async Task<HairdresserWorkingHoursDto> GetCurrentSelfWorkingHoursAsync(CancellationToken ct)
    {
        return await api.GetAsync<HairdresserWorkingHoursDto>("api/me/working-hours", ct);
    }

    public async Task<HairdresserWorkingHoursDto> GetForHairdresserByIdAsync(string hairdresserId, CancellationToken ct)
    {
        return await api.GetAsync<HairdresserWorkingHoursDto>($"api/admin/working-hours/{hairdresserId}", ct);
    }

    public async Task UpdateSelfWorkingHoursAsync(HairdresserWorkingHoursUpsertDto request, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Put, "api/me/working-hours", request, ct);
    }

    public async Task<HairdresserWorkingHoursDto> GetNextForHairdresserByIdAsync(string hairdresserId, CancellationToken ct)
    {
        return await api.GetAsync<HairdresserWorkingHoursDto>($"api/admin/working-hours/{hairdresserId}/upcoming", ct);
    }

    public async Task<HairdresserWorkingHoursDto> GetNextSelfWorkingHoursAsync(CancellationToken ct)
    {
        return await api.GetAsync<HairdresserWorkingHoursDto>("api/me/working-hours/upcoming", ct);
    }

    public Task<WorkingHoursDto> GetByDayAsync(DateOnly day, CancellationToken ct)
    {
        return api.GetAsync<WorkingHoursDto>($"api/me/working-hours/daily?day={day:yyyy-MM-dd}", ct);
    }

    public async Task<List<WorkingHoursDto>> GetWorkingHoursByWeek(string hairdresserId, DateOnly monday, CancellationToken ct)
    {
        var url = string.Empty;

        await authState.LoadAsync();

        if (authState.Roles.Contains(nameof(UserRoles.Admin)))
            url = "api/admin/working-hours/selected-week";
        else if (authState.Roles.Contains(nameof(UserRoles.Hairdresser)))
            url = "api/me/working-hours/selected-week";
        else
            throw new ApiRequestException("Nelze zobrazit kalendnář - nemáte právo.", StatusCodes.Status403Forbidden);

        string parameters;

        if (url.StartsWith("api/me"))
        {
            parameters = $"monday={monday.ToString("yyyy-MM-dd")}";
        }
        else
        {
            parameters = $"hairdresserId={Uri.EscapeDataString(hairdresserId.Trim())}&monday={monday.ToString("yyyy-MM-dd")}";
        }

        return await api.GetAsync<List<WorkingHoursDto>>($"{url}?{parameters}", ct);
    }

    public async Task<bool> ExistsWorkingHoursAsync(CancellationToken ct)
    {
        await authState.LoadAsync();

        if (!authState.IsAuthenticated)
            return false;

        if (!authState.Roles.Contains(nameof(UserRoles.Hairdresser)))
            return false;

        return await api.GetAsync<bool>("api/me/working-hours/exists", ct);
    }
}
