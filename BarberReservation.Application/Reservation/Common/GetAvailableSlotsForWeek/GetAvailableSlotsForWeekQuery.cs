using BarberReservation.Shared.Models.Reservation;
using MediatR;

namespace BarberReservation.Application.Reservation.Common.GetAvailableSlotsForWeek;

public sealed class GetAvailableSlotsForWeekQuery(string hairdresserId, DateTime weekStartDate, int serviceId) : IRequest<IReadOnlyList<SlotDto>>
{
    public string HairdresserId { get; init; } = hairdresserId;
    public int ServiceId { get; init; } = serviceId;
    public DateTime WeekStartDate { get; init; } = weekStartDate;
}
