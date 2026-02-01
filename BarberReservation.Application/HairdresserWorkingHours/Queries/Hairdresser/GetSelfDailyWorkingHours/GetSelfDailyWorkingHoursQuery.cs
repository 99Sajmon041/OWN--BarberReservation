using BarberReservation.Application.Common.Security;
using BarberReservation.Shared.Models.HairdresserWorkingHours;
using MediatR;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetSelfDailyWorkingHours;

public sealed class GetSelfDailyWorkingHoursQuery(DateOnly day) : IRequireActiveUser, IRequest<WorkingHoursDto>
{
    public DateOnly Day { get; } = day;
}
