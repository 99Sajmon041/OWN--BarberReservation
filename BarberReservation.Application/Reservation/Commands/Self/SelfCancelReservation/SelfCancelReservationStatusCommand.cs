using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Common.Validation.IdValidation;
using MediatR;

namespace BarberReservation.Application.Reservation.Commands.Self.SelfCancelReservation;

public sealed class SelfCancelReservationStatusCommand(int id) : IHasId, IRequireActiveUser, IRequest
{
    public int Id { get; init; } = id;
}
