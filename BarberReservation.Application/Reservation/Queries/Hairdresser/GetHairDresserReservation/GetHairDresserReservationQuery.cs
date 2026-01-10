using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Models.Reservation.Hairdresser;
using MediatR;

namespace BarberReservation.Application.Reservation.Queries.Hairdresser.GetHairDresserReservation;

public sealed class GetHairDresserReservationQuery(int id) : IHasId, IRequest<HairdresserReservationDto>
{
    public int Id { get; init; } = id;
}
