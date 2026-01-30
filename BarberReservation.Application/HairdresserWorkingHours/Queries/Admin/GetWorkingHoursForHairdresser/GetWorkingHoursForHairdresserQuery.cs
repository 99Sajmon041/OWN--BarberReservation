using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;
using MediatR;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetWorkingHoursForHairdresser;

public sealed class GetWorkingHoursForHairdresserQuery(string hairdresserId) : IRequest<HairdresserWorkingHoursDto>
{
    public string HairdresserId { get; init; } = hairdresserId;
}
