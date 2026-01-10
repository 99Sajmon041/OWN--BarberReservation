using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Models.Reservation.Admin;
using MediatR;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAdminReservation;

public sealed class GetAdminReservationQuery(int id) : IHasId, IRequest<AdminReservationDto>
{
    public int Id { get; init; } = id;
}
