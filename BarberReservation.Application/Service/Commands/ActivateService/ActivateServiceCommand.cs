using BarberReservation.Application.Common.Validation.IdValidation;
using MediatR;

namespace BarberReservation.Application.Service.Commands.ActivateService;

public sealed class ActivateServiceCommand(int id) : IRequest, IHasId
{
    public int Id { get; init; } = id;
}
