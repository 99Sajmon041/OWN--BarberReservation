using BarberReservation.Shared.Models.HairdresserWorkingHours.Admin;
using MediatR;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetWorkingHoursForHairdresser;

public sealed class GetWorkingHoursForHairdresserQuery(string hairdresserId) : IRequest<IReadOnlyList<AdminHairdresserWorkingHoursDto>>
{
    public string HairdresserId { get; init; } = hairdresserId;
}
