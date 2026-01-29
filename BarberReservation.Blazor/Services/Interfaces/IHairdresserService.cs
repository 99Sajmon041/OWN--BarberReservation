using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface IHairdresserService
{
    Task<PagedResult<HairdresserServiceDto>> GetAllAsync(CommonHairdresserServicePagedRequest request, CancellationToken ct);
    Task<HairdresserServiceDto> GetByIdAsync(int id, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
    Task DeactivateAsync(int id, CancellationToken ct);
    Task ActivateAsync(int id, CancellationToken ct);
}
