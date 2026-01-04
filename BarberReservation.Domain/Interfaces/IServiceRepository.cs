using BarberReservation.Domain.Entities;

namespace BarberReservation.Domain.Interfaces;

public interface IServiceRepository
{
    Task CreateAsync(Service service, CancellationToken ct);
    Task<Service?> GetByIdAsync(int id, CancellationToken ct);
    Task<(IReadOnlyList<Service>, int)> GetAllAsync(int page, int pageSize, bool? isActive, string? search, string? sortBy, bool desc, CancellationToken ct);
    bool Deactivate(Service service);
}
