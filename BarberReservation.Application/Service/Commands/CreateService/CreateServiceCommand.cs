using MediatR;

namespace BarberReservation.Application.Service.Commands.CreateService;

public sealed class CreateServiceCommand : IRequest<int>
{
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}
