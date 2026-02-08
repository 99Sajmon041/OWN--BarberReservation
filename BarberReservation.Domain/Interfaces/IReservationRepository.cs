using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Models.Reservation;

namespace BarberReservation.Domain.Interfaces;

public interface IReservationRepository
{
    Task<bool> ExistsByHairDresserServiceIdAsync(int hairDresserServiceid, CancellationToken ct);
    Task<(IReadOnlyList<Reservation>, int)> GetPagedForAdminAsync(ReservationPagedRequest request, CancellationToken ct);
    Task<(IReadOnlyList<Reservation>, int)> GetPagedForHairdresserAsync(ReservationPagedRequest request, string hairDresserId, CancellationToken ct);
    Task<(IReadOnlyList<Reservation>, int)> GetPagedForClientAsync(ReservationPagedRequest request, string userId, CancellationToken ct);
    Task<Reservation?> GetForAdminAsync(int id, CancellationToken ct);
    Task<Reservation?> GetForHairdresserAsync(int id, string hairDresserId, CancellationToken ct);
    Task<Reservation?> GetForClientAsync(int id, string userId, CancellationToken ct);
    Task CreateAsync(Reservation reservation, CancellationToken ct);
    Task<bool> ExistsOverlapForHairdresserAsync(string hairdresserId, DateTime startAt, DateTime endAt, CancellationToken ct);
    Task<bool> ExistAnyUpComingReservationForClientAsync(string userId, CancellationToken ct);
    Task<bool> ExistAnyUpComingReservationForHairdresserAsync(string hairdresserId, CancellationToken ct);
    Task<List<Reservation>> GetForHairdresserDailyAsync(string hairdresserId, DateOnly day, CancellationToken ct);
}
