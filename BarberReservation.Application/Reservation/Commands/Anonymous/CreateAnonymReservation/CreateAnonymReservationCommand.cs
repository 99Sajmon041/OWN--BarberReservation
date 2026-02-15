using BarberReservation.Application.Reservation.Common.Interfaces;
using MediatR;

namespace BarberReservation.Application.Reservation.Commands.Anonymous.CreateAnonymReservation;

public sealed class CreateAnonymReservationCommand : IReservationCreate, IRequest
{
    public string HairdresserId { get; set; } = default!;
    public int ServiceId { get; set; }
    public DateTime StartAt { get; set; }
    public string CustomerName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerPhone { get; set; } = default!;
}
