using MediatR;

namespace BarberReservation.Application.Service.Commands.DeactivateService;

public sealed class DeactivateServiceCommand(int id) : IRequest
{
    public int Id { get; init; } = id;
}
