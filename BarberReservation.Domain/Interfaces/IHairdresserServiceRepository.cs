using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Models.HairdresserService;

namespace BarberReservation.Domain.Interfaces;

public interface IHairdresserServiceRepository
{
    Task<bool> ExistsByServiceIdAsync(int serviceId, CancellationToken ct);
    Task<HairdresserService?> GetByIdForAdminAsync(int id, CancellationToken ct);
    Task<HairdresserService?> GetByIdForCurrentUserAsync(int id, string hairdresserId, CancellationToken ct);
    Task<HairdresserService?> GetByIdForClientAsync(int id, string hairdresserId, CancellationToken ct);
    Task CreateAsync(HairdresserService hairdresserService, CancellationToken ct);
    Task<bool> ExistsActiveWithSameServiceAsync(string hairdresserId, int serviceId, CancellationToken ct);
    bool Deactivate(HairdresserService hairdresserService);
    void Delete(HairdresserService hairdresserService);
    Task<(IReadOnlyList<HairdresserService>, int)> GetAllPagedForAdminAsync(CommonHairdresserServicePagedRequest request, CancellationToken ct);
    Task<(IReadOnlyList<HairdresserService>, int)> GetAllPagedForCurrentUserAsync(
        CommonHairdresserServicePagedRequest request, 
        string userId, 
        CancellationToken ct);
    Task<IReadOnlyList<string>> GetActiveHairdresserIdsByServiceIdAsync(int id, CancellationToken ct);
}
