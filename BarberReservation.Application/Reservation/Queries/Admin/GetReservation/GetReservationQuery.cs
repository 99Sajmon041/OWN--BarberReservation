using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Models.Rezervation.Admin;
using MediatR;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetReservation;

public sealed class GetReservationQuery(int id) : IHasId, IRequest<AdminReservationDto>
{
    public int Id { get; init; } = id;
}
