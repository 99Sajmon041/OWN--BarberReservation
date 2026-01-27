using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Service;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface IService
{
    Task<PagedResult<ServiceDto>> GetAllAsync(ServicePageRequest request, CancellationToken ct);
    Task<ServiceDto> GetByIdAsync(int id, CancellationToken ct);
    Task DeactivateByIdAsync(int id, CancellationToken ct);
    Task ActivateByIdAsync(int id, CancellationToken ct);
    Task<int> CreateAsync(UpsertServiceRequest request, CancellationToken ct);
    Task UpdateAsync(int id, UpsertServiceRequest request, CancellationToken ct);
}
