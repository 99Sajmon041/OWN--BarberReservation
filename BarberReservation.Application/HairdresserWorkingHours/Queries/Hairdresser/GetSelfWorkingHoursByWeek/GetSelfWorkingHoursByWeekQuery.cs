using BarberReservation.Application.Common.Security;
using BarberReservation.Application.HairdresserWorkingHours.Common;
using BarberReservation.Shared.Models.HairdresserWorkingHours;
using MediatR;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetSelfWorkingHoursByWeek;

public sealed class GetSelfWorkingHoursByWeekQuery(DateOnly monday) : IWorkingHoursWeekFilter, IRequireActiveUser, IRequest<List<WorkingHoursDto>>
{
    public DateOnly Monday { get; init; } = monday;
}
