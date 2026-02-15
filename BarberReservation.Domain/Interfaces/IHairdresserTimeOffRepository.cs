using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Models.TimeOff;

namespace BarberReservation.Domain.Interfaces;

public interface IHairdresserTimeOffRepository
{
    Task<bool> ExistsOverlapAsync(string hairdresserId, DateTime startAt, DateTime endAt, CancellationToken ct);
    Task<IReadOnlyList<HairdresserTimeOff>> GetTimeOffFromDateAsync(string hairdresserId, DateOnly fromDate, CancellationToken ct);
    Task<(IReadOnlyList<HairdresserTimeOff>, int)> GetAllPagedForAdminAsync(HairdresserTimeOffPagedRequest request, CancellationToken ct);
    Task<(IReadOnlyList<HairdresserTimeOff>, int)> GetAllPagedForHairdresserAsync(string hairdresserId, HairdresserTimeOffPagedRequest request, CancellationToken ct);
    void Add(HairdresserTimeOff timeOff);
    Task<HairdresserTimeOff?> GetByIdAsync(int id, string hairdresserId, CancellationToken ct);
    Task<List<HairdresserTimeOff>> GetAllByDayForHairdresserAsync(string hairdresserId, DateOnly day, CancellationToken ct);
    void Delete(HairdresserTimeOff hairdresserTimeOff);
    Task<IReadOnlyList<HairdresserTimeOff>> GetAllWeeklyAsync(string hairdresserId, DateTime weekStartDate, CancellationToken ct);
}
