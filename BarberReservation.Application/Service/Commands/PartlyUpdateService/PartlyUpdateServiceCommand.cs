using MediatR;

namespace BarberReservation.Application.Service.Commands.PartlyUpdateService;

public sealed class PartlyUpdateServiceCommand : IRequest
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}
