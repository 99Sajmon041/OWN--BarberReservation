using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Common.Validation.IdValidation;
using MediatR;

namespace BarberReservation.Application.TimeOff.Commands.Hairdresser.DeleteTimeOff;

public sealed class DeleteTimeOffCommand(int id) : IHasId, IRequireActiveUser, IRequest
{
    public int Id { get; init; } = id;
}
