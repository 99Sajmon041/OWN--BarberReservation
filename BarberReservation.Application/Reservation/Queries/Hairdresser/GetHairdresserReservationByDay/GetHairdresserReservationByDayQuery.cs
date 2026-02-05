using BarberReservation.Application.Common.Security;
using BarberReservation.Shared.Models.Reservation.Common;
using MediatR;

namespace BarberReservation.Application.Reservation.Queries.Hairdresser.GetHairdresserReservationByDay;

public sealed class GetHairdresserReservationByDayQuery(DateOnly day) : IRequireActiveUser, IRequest<List<ReservationDto>>
{
    public DateOnly Day { get; } = day;
}
