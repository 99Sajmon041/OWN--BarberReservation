using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Models.HairdresserWorkingHours;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;

namespace BarberReservation.Blazor.Services.Implementaions;

public sealed class WorkingHoursService(IApiClient api) : IWorkingHoursService
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
}
