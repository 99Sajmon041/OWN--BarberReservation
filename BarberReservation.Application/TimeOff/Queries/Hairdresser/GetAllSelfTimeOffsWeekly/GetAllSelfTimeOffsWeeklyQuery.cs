using BarberReservation.Application.Common.Security;
using BarberReservation.Application.TimeOff.Common;
using BarberReservation.Shared.Models.TimeOff;
using MediatR;

namespace BarberReservation.Application.TimeOff.Queries.Hairdresser.GetAllSelfTimeOffsWeekly;

public sealed class GetAllSelfTimeOffsWeeklyQuery(DateTime weekStartDate) : IRequireActiveUser, ITimeOffDailyListFilter, IRequest<List<HairdresserTimeOffDto>>
{
    public DateTime WeekStartDate { get; init; } = weekStartDate;
}
