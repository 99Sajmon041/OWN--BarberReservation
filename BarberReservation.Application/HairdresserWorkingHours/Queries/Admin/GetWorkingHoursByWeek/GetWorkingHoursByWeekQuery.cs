using BarberReservation.Application.HairdresserWorkingHours.Common;
using BarberReservation.Shared.Models.HairdresserWorkingHours;
using MediatR;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetWorkingHoursByWeek;

public sealed class GetWorkingHoursByWeekQuery(string hairdresserId, DateOnly monday) : IWorkingHoursWeekFilter, IRequest<List<WorkingHoursDto>>
{
    public string HairdresserId { get; init; } = hairdresserId;
    public DateOnly Monday { get; init; } = monday;
}
