using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;
using MediatR;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetNextWorkingHoursForHairdresser;

public sealed class GetNextWorkingHoursForHairdresserQuery(string hairdresserId) : IRequest<HairdresserWorkingHoursDto>
{
    public string HairdresserId { get; } = hairdresserId;
}
