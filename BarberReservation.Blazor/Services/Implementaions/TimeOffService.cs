using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Models.TimeOff.Common;
using BarberReservation.Shared.Models.TimeOff.Hairdresser;

namespace BarberReservation.Blazor.Services.Implementaions;

public sealed class TimeOffService(IApiClient api) : ITimeOffService
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
}
