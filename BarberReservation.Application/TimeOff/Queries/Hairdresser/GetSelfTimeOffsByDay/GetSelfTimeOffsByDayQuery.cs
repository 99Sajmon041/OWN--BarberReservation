using BarberReservation.Application.Common.Security;
using BarberReservation.Shared.Models.TimeOff;
using MediatR;

namespace BarberReservation.Application.TimeOff.Queries.Hairdresser.GetSelfTimeOffsByDay;

public sealed class GetSelfTimeOffsByDayQuery : IRequireActiveUser, IRequest<List<HairdresserTimeOffDto>> 
{
    public DateOnly Day { get; init; }
}
