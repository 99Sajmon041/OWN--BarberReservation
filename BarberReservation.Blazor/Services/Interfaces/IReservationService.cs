using BarberReservation.Shared.Models.Reservation.Hairdresser;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface IReservationService
{
    Task<List<HairdresserReservationDto>> GetByDayAsync(DateOnly day, CancellationToken ct);
}
