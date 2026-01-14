using BarberReservation.Application.Common.Security;
using MediatR;

namespace BarberReservation.Application.HairdresserWorkingHours.Commands.Hairdresser.UpsertSelfWorkingHours;

public sealed class UpsertSelfWorkingHoursCommand : IRequireActiveUser, IRequest
{
    public IEnumerable<UpsertHairdresserWorkingHoursDto> DaysOfWorkingWeek { get; init; } = [];
}
