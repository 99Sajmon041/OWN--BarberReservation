using MediatR;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.NewFolder;

public sealed class DeactivateHairdresserServiceCommand(int id) : IRequest
{
    public int Id { get; init; } = id;
}
