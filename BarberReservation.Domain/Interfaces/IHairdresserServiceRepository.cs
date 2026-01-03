using BarberReservation.Domain.Entities;

namespace BarberReservation.Domain.Interfaces;

public interface IHairdresserServiceRepository
{
    Task<bool> ExistsByServiceIdAsync(int serviceId, CancellationToken ct);
    Task<(IReadOnlyList<HairdresserService>, int)> GetAllPagedForAdminAsync(int page, int pageSize, string? hairdresserId, int? serviceId, string? sortBy, bool desc, bool? isActive, CancellationToken ct);
    Task<HairdresserService?> GetByIdForAdminAsync(int id, CancellationToken ct);
    void Delete(HairdresserService hairdresserService);
}
