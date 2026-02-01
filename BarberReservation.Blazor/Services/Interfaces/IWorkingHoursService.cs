using BarberReservation.Shared.Models.HairdresserWorkingHours;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface IWorkingHoursService
{
    Task UpdateSelfWorkingHoursAsync(HairdresserWorkingHoursUpsertDto request, CancellationToken ct);
    Task<HairdresserWorkingHoursDto> GetCurrentSelfWorkingHoursAsync(CancellationToken ct);
    Task<HairdresserWorkingHoursDto> GetForHairdresserByIdAsync(string hairdresserId, CancellationToken ct);
    Task<HairdresserWorkingHoursDto> GetNextSelfWorkingHoursAsync(CancellationToken ct);
    Task<HairdresserWorkingHoursDto> GetNextForHairdresserByIdAsync(string hairdresserId, CancellationToken ct);
    Task<WorkingHoursDto> GetByDayAsync(DateOnly day, CancellationToken ct);
}
