using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Models.Reservation.Common;
using MediatR;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAdminReservation;

public sealed class GetAdminReservationQuery(int id) : IHasId, IRequest<ReservationDto>
{
    public int Id { get; init; } = id;
}
