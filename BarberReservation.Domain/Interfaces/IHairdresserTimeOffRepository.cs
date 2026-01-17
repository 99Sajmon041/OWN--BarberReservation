using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Models.TimeOff.Admin;
using BarberReservation.Shared.Models.TimeOff.Hairdresser;

namespace BarberReservation.Domain.Interfaces;

public interface IHairdresserTimeOffRepository
{
    Task<bool> ExistsOverlapAsync(string hairdresserId, DateTime startAt, DateTime endAt, CancellationToken ct);
    Task<IReadOnlyList<HairdresserTimeOff>> GetTimeOffFromDateAsync(string hairdresserId, DateOnly fromDate, CancellationToken ct);
    Task<(IReadOnlyList<HairdresserTimeOff>, int)> GetAllPagedForAdminAsync(AdminHairdresserPagedRequest request, int? year, int? month, CancellationToken ct);
    Task<(IReadOnlyList<HairdresserTimeOff>, int)> GetAllPagedForHairdresserAsync(
        string hairdresserId, 
        HairdresserPagedRequest request,
        int? year,
        int? month,
        CancellationToken ct);
    void Add(HairdresserTimeOff timeOff);
    Task<HairdresserTimeOff?> GetByIdAsync(int id, string hairdresserId, CancellationToken ct);
    Task<IReadOnlyList<HairdresserTimeOff>> GetAllByDayForHairdresserAsync(string hairdresserId, DateTime today, CancellationToken ct);
    void Delete(HairdresserTimeOff hairdresserTimeOff);
}
