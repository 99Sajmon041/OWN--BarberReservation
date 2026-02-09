using BarberReservation.Application.Reservation.Common.Interfaces;
using MediatR;

namespace BarberReservation.Application.Reservation.Commands.Admin.CreateAdminReservation;

public sealed class CreateAdminReservationCommand : IReservationCreate, IRequest<int>
{
    public string HairdresserId { get; set; } = default!;
    public int HairdresserServiceId { get; set; }
    public DateTime StartAt { get; set; }
    public string? CustomerId { get; set; }
    public string CustomerName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerPhone { get; set; } = default!;
}
