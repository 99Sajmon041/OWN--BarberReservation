using BarberReservation.Domain.Entities;

namespace BarberReservation.Domain.Interfaces;

public interface IHairdresserWorkingHoursRepository
{
    Task<HairdresserWorkingHours?> GetForDayAsync(string hairdresserId, DayOfWeek dayOfWeek, CancellationToken ct);
    Task<IReadOnlyList<HairdresserWorkingHours>> GetCurrentWeekAsync(
        string hairdresserId,
        DateOnly onDate,
        bool includeHairdresser,
        bool tracked,
        CancellationToken ct);

    Task<IReadOnlyList<HairdresserWorkingHours>> GetNextWeekAsync(
        string hairdresserId, 
        DateOnly onDate,
        bool includeHairdresser, 
        bool tracked,
        CancellationToken ct);
    void AddDaysToWorkingWeek(IEnumerable<HairdresserWorkingHours> days);
    Task<List<HairdresserWorkingHours>> GetWeekByEffectiveFromAsync(string hairdresserId, DateOnly validFromDate, CancellationToken ct);
    Task<HairdresserWorkingHours?> GetEffectiveFromDayByTimeOffAsync(string hairdresserId, DateOnly TimeOffDay, CancellationToken ct);
    Task<HairdresserWorkingHours?> GetEffectiveForDayAsync(string hairdresserId, DayOfWeek dayOfWeek, DateOnly day, CancellationToken ct);
    Task<List<HairdresserWorkingHours>> GetWeekAsync(string hairdresserId, DateOnly monday, CancellationToken ct);
    Task<List<HairdresserLookupRow>> GetActiveHairdressersWithAnyFullWeekAsync(CancellationToken ct);
    Task<bool> ExistWorkingHoursByHairdresser(string hairdresserId, CancellationToken ct);

}
