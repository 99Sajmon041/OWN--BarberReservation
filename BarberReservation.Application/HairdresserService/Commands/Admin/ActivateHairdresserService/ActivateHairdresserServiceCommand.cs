using BarberReservation.Application.Common.Validation.IdValidation;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.ActivateHairdresserService;

public sealed class ActivateHairdresserServiceCommand(int id) : IHasId, IRequest
{
    public int Id { get; init; } = id;
}
