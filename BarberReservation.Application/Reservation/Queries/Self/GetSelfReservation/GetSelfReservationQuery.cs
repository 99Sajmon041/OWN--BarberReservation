using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Models.Reservation;
using MediatR;

namespace BarberReservation.Application.Reservation.Queries.Self.GetSelfReservation;

public sealed class GetSelfReservationQuery(int id) : IHasId, IRequireActiveUser, IRequest<ReservationDto>
{
    public int Id { get; init; } = id;
}
