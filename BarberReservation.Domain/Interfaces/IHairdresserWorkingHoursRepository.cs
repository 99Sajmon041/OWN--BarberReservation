using BarberReservation.Domain.Entities;

namespace BarberReservation.Domain.Interfaces;

public interface IHairdresserWorkingHoursRepository
{
    Task<HairdresserWorkingHours?> GetForDayAsync(string hairdresserId, DayOfWeek dayOfWeek, CancellationToken ct);
}
