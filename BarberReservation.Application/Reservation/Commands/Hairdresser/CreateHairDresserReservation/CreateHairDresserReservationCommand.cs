using BarberReservation.Application.Reservation.Common;
using MediatR;

namespace BarberReservation.Application.Reservation.Commands.Hairdresser.CreateHairDresserReservation;

public sealed class CreateHairDresserReservationCommand : IReservationCreate, IRequest<int>
{
    public int HairDresserServiceId { get; set; }
    public DateTime StartAt { get; set; }
    public string? CustomerId { get; set; }
    public string CustomerName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerPhone { get; set; } = default!;
}
