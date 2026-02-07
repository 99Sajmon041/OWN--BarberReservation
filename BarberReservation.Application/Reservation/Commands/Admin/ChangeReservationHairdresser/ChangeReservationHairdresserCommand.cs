using MediatR;

namespace BarberReservation.Application.Reservation.Commands.Admin.ChangeReservationHairdresser;

public sealed class ChangeReservationHairdresserCommand(int reservationId, string hairdresserId) : IRequest
{
    public int ReservationId { get; } = reservationId;
    public string HairdresserId { get; } = hairdresserId;
}
