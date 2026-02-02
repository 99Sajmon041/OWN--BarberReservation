using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.TimeOff;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface ITimeOffService
{
    Task<List<HairdresserTimeOffDto>> GetByDayAsync(DateOnly day, CancellationToken ct);
    Task CreateAsync(UpsertTimeOffRequest request, CancellationToken ct);
    Task UpdateAsync(int id, UpsertTimeOffRequest request, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
    Task<PagedResult<HairdresserTimeOffDto>> GetAllAsync(HairdresserPagedRequest request, CancellationToken ct);
}
