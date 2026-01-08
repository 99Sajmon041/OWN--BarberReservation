using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Models.Service;

namespace BarberReservation.Domain.Interfaces;

public interface IServiceRepository
{
    Task CreateAsync(Service service, CancellationToken ct);
    Task<Service?> GetByIdAsync(int id, CancellationToken ct);
    Task<(IReadOnlyList<Service>, int)> GetAllAsync(ServicePageRequest request, CancellationToken ct);
    bool Deactivate(Service service);
    Task<bool> ExistsAsync(int id, CancellationToken ct);
 }
