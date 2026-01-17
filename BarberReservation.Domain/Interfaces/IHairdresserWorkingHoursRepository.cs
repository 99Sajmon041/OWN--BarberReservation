using BarberReservation.Domain.Entities;

namespace BarberReservation.Domain.Interfaces;

public interface IHairdresserWorkingHoursRepository
{
    Task<HairdresserWorkingHours?> GetForDayAsync(string hairdresserId, DayOfWeek dayOfWeek, CancellationToken ct);
    Task<IReadOnlyList<HairdresserWorkingHours>> GetWeekAsync(
        string hairdresserId,
        DateOnly onDate,
        bool includeHairdresser,
        bool tracked,
        CancellationToken ct);
    void AddDaysToWorkingWeek(IEnumerable<HairdresserWorkingHours> days);
    Task<List<HairdresserWorkingHours>> GetWeekByEffectiveFromAsync(string hairdresserId, DateOnly validFromDate, CancellationToken ct);
    Task<HairdresserWorkingHours?> GetEffectiveFromDayByTimeOffAsync(string hairdresserId, DateOnly TimeOffDay, CancellationToken ct);
}
