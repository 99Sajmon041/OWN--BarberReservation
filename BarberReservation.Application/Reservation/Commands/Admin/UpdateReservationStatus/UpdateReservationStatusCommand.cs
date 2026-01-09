using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Enums;
using MediatR;

namespace BarberReservation.Application.Reservation.Commands.Admin.UpdateReservationStatus;

public sealed class UpdateReservationStatusCommand : IHasId, IRequest
{
    public int Id { get; init; }
    public ReservationStatus NewReservationStatus { get; init; }
    public CanceledReason? CanceledReason { get; init; }
}
