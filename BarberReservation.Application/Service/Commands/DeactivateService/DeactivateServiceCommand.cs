using BarberReservation.Application.Common.Validation.IdValidation;
using MediatR;

namespace BarberReservation.Application.Service.Commands.DeactivateService;

public sealed class DeactivateServiceCommand(int id) : IRequest, IHasId
{
    public int Id { get; init; } = id;
}
