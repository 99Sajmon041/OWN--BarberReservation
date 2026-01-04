using BarberReservation.Application.Common.Validation.IdValidatikon;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.DeleteHairdresserService;

public sealed class DeleteHairdresserServiceCommand(int id) : IRequest, IHasId
{
    public int Id { get; init; } = id;
}
