using BarberReservation.Application.Reservation.Common.Interfaces;
using BarberReservation.Shared.Models.Reservation;
using MediatR;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAllWeeklyAdminReservations;

public sealed class GetAllWeeklyAdminReservationsQuery(DateTime monday, string hairdresserId) : IReservationsWeekFilter, IRequest<List<ReservationDto>>
{
    public DateTime Monday { get; init; } = monday;
    public string HairdresserId { get; init; } = hairdresserId;
}
