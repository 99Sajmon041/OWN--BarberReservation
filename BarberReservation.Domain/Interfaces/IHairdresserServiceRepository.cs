using BarberReservation.Domain.Entities;

namespace BarberReservation.Domain.Interfaces;

public interface IHairdresserServiceRepository
{
    Task<bool> ExistsByServiceIdAsync(int serviceId, CancellationToken ct);
    Task<HairdresserService?> GetByIdForAdminAsync(int id, CancellationToken ct);
    Task<HairdresserService?> GetByIdForCurrentUserAsync(int id, string hairdresserId, CancellationToken ct);
    Task CreateAsync(HairdresserService hairdresserService, CancellationToken ct);
    Task<bool> ExistsActiveWithSameServiceAsync(string hairdresserId, int serviceId, CancellationToken ct);
    bool Deactivate(HairdresserService hairdresserService);
    void Delete(HairdresserService hairdresserService);
    Task<(IReadOnlyList<HairdresserService>, int)> GetAllPagedForAdminAsync(
        int page,
        int pageSize,
        string? hairdresserId,
        int? serviceId,
        string? search,
        string? sortBy,
        bool desc,
        bool? isActive,
        CancellationToken ct);

    Task<(IReadOnlyList<HairdresserService>, int)> GetAllPagedForCurrentUserAsync(
        int page,
        int pageSize,
        string userId,
        int? serviceId,
        string? search,
        string? sortBy,
        bool desc,
        bool? isActive,
        CancellationToken ct);
}
