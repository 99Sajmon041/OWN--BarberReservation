using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation.Common;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface IReservationService
{
    Task<List<ReservationDto>> GetByDayAsync(DateOnly day, CancellationToken ct);
    Task<PagedResult<ReservationDto>> GetAllAsync(ReservationPagedRequest request, CancellationToken ct);
    Task UpdateStatusAsync(int id, UpdateReservationRequest request, CancellationToken ct);
    Task CancelReservationByClientAsync(int id, CancellationToken ct);
}
