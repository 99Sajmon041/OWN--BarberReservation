using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Reservation.Common.Interfaces;
using MediatR;

namespace BarberReservation.Application.Reservation.Commands.Hairdresser.CreateHairDresserReservation;

public sealed class CreateHairDresserReservationCommand : IReservationCreate, IRequireActiveUser, IRequest<int>
{
    public int ServiceId { get; set; }
    public DateTime StartAt { get; set; }
    public string? CustomerId { get; set; }
    public string CustomerName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerPhone { get; set; } = default!;
}
