using BarberReservation.Application.Common.Validation.IdValidatikon;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.NewFolder;

public sealed class DeactivateHairdresserServiceCommand(int id) : IRequest, IHasId
{
    public int Id { get; init; } = id;
}
