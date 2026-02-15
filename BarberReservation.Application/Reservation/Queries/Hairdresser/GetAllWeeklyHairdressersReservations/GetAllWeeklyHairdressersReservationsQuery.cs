using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Reservation.Common.Interfaces;
using BarberReservation.Shared.Models.Reservation;
using MediatR;

namespace BarberReservation.Application.Reservation.Queries.Hairdresser.GetAllWeeklyHairdressersReservations;

public sealed class GetAllWeeklyHairdressersReservationsQuery(DateTime monday) : IRequireActiveUser, IReservationsWeekFilter, IRequest<List<ReservationDto>>
{
    public DateTime Monday { get; init; } = monday;
}
