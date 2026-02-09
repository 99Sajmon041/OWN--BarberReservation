using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Application.Reservation.Common.Interfaces;
using BarberReservation.Shared.Enums;
using MediatR;

namespace BarberReservation.Application.Reservation.Commands.Admin.UpdateAdminReservationStatuss;

public sealed class UpdateAdminReservationStatusCommand : IHasId, IRequest, IReservationStatusUpdate
{
    public int Id { get; init; }
    public ReservationStatus NewReservationStatus { get; init; }
    public CanceledReason? CanceledReason { get; init; }
}
