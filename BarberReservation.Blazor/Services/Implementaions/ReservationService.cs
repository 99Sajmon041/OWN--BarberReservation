using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Models.Reservation.Hairdresser;

namespace BarberReservation.Blazor.Services.Implementaions;

public sealed class ReservationService(IApiClient api) : IReservationService
{
    public async Task<List<HairdresserReservationDto>> GetByDayAsync(DateOnly day, CancellationToken ct)
    {
        return await api.GetAsync<List<HairdresserReservationDto>>($"api/hairdresser/reservations/daily?day={day:yyyy-MM-dd}", ct);
    }
}
