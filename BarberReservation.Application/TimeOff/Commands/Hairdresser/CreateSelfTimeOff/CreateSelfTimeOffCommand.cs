using BarberReservation.Application.Common.Security;
using BarberReservation.Application.TimeOff.Common;
using MediatR;

namespace BarberReservation.Application.TimeOff.Commands.Hairdresser.CreateSelfTimeOff;

public sealed class CreateSelfTimeOffCommand : IRequireActiveUser, ITimeOffUpsert, IRequest
{
    public DateTime StartAt { get; init; }
    public DateTime EndAt { get; init; }
    public string Reason { get; init; } = default!;
}
