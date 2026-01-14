using BarberReservation.Domain.Entities;

namespace BarberReservation.Domain.Interfaces;

public interface IHairdresserWorkingHoursRepository
{
    Task<HairdresserWorkingHours?> GetForDayAsync(string hairdresserId, DayOfWeek dayOfWeek, CancellationToken ct);
    Task<IReadOnlyList<HairdresserWorkingHours>> GetAllDaysInWeekForHairdresser(
        string hairdresserId,
        bool tracked, bool includeHairdresser,
        CancellationToken ct);
    void AddDayToWorkingWeek(HairdresserWorkingHours day);
}
