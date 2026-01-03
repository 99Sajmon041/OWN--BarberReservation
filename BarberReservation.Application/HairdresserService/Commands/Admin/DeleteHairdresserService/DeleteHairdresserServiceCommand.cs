using MediatR;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.DeleteHairdresserService;

public sealed class DeleteHairdresserServiceCommand(int id) : IRequest
{
    public int Id { get; init; } = id;
}
