using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Rezervation.Common;

namespace BarberReservation.Domain.Interfaces;

public interface IReservationRepository
{
    Task<bool> ExistsByHairDresserServiceIdAsync(int hairDresserServiceid, CancellationToken ct);
    Task<(IReadOnlyList<Reservation>, int)> GetPagedForAdminAsync(AdminReservationPagedRequest request, CancellationToken ct);
    Task<Reservation?> GetForAdminAsync(int id, CancellationToken ct);
    Task CreateForAdminAsync(Reservation reservation, CancellationToken ct);
}
