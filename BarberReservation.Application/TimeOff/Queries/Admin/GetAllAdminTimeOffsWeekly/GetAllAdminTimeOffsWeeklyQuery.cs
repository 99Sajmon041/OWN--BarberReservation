using BarberReservation.Application.TimeOff.Common;
using BarberReservation.Shared.Models.TimeOff;
using MediatR;

namespace BarberReservation.Application.TimeOff.Queries.Admin.GetAllAdminTimeOffsWeekly;

public sealed class GetAllAdminTimeOffsWeeklyQuery(DateTime weekStartDate, string hairdresserId) : ITimeOffDailyListFilter, IRequest<List<HairdresserTimeOffDto>>
{
    public DateTime WeekStartDate { get; init; } = weekStartDate;
    public string HairdresserId { get; init; } = hairdresserId;
}
