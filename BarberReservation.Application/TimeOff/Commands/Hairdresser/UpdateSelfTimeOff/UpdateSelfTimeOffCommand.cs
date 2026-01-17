using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Application.TimeOff.Common;
using MediatR;

namespace BarberReservation.Application.TimeOff.Commands.Hairdresser.UpdateSelfTimeOff;

public sealed class UpdateSelfTimeOffCommand(int id) : IRequireActiveUser, ITimeOffUpsert, IHasId, IRequest
{
    public int Id { get; init; } = id;
    public DateTime StartAt { get; init; }
    public DateTime EndAt { get; init; }
    public string Reason { get; init; } = default!;
}
