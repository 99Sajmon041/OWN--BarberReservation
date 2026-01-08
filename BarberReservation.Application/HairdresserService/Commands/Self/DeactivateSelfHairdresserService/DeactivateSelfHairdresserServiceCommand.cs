using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Common.Validation.IdValidation;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Commands.Self.DeactivateSelfHairdresserService;

public sealed class DeactivateSelfHairdresserServiceCommand(int id) : IRequest, IRequireActiveUser, IHasId
{
    public int Id { get; init; } = id;
}
