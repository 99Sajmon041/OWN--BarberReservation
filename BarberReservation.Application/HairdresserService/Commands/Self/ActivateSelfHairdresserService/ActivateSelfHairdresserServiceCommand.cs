using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Common.Validation.IdValidation;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Commands.Self.ActivateSelfHairdresserService;

public sealed class ActivateSelfHairdresserServiceCommand(int id) : IRequireActiveUser, IHasId, IRequest
{
    public int Id { get; init; } = id;
}
