using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface IHairdresserService
{
    Task<PagedResult<HairdresserServiceDto>> GetAllAsync(CommonHairdresserServicePagedRequest request, CancellationToken ct);
}
