using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation;
using System.Reflection;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface IReservationService
{
    Task<List<ReservationDto>> GetByDayAsync(DateOnly day, CancellationToken ct);
    Task<PagedResult<ReservationDto>> GetAllAsync(ReservationPagedRequest request, CancellationToken ct);
    Task<ReservationDto> GetByIdAsync(int id, CancellationToken ct);
    Task UpdateStatusAsync(int id, UpdateReservationStatusRequest request, CancellationToken ct);
    Task CancelReservationByClientAsync(int id, CancellationToken ct);
    Task ChangeReservationHairdresserAsync(int reservationId, string hairdresserId, CancellationToken ct);
    Task<int> CreateReservationAsync(CreateReservationRequest request, CancellationToken ct);
    Task CreateReservationAsCustomerAsync(CreateReservationRequest request, CancellationToken ct);
    Task<List<SlotDto>> GetFreeSlotsForWeekAsync(string hairdresserId, DateTime weekStartDate, int serviceId, CancellationToken ct);
}
