using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Application.Reservation.Common;
using BarberReservation.Shared.Enums;
using MediatR;

namespace BarberReservation.Application.Reservation.Commands.Hairdresser.UpdateHairDresserReservationStatus;

public sealed class UpdateHairDresserReservationStatusCommand : IHasId, IRequest, IReservationStatusUpdate
{
    public int Id { get; init; }
    public ReservationStatus NewReservationStatus { get; init; }
    public CanceledReason? CanceledReason { get; init; }
}
