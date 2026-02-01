using BarberReservation.Shared.Models.TimeOff.Common;
using BarberReservation.Shared.Models.TimeOff.Hairdresser;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface ITimeOffService
{
    Task<List<HairdresserTimeOffDto>> GetByDayAsync(DateOnly day, CancellationToken ct);
    Task CreateAsync(UpsertTimeOffRequest request, CancellationToken ct);
    Task UpdateAsync(int id, UpsertTimeOffRequest request, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}
