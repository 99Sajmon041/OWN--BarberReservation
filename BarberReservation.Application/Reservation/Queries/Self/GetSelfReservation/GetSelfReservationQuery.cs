using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Models.Reservation.Self;
using MediatR;

namespace BarberReservation.Application.Reservation.Queries.Self.GetSelfReservation;

public sealed class GetSelfReservationQuery(int id) : IHasId, IRequireActiveUser, IRequest<SelfReservationDto>
{
    public int Id { get; init; } = id;
}
