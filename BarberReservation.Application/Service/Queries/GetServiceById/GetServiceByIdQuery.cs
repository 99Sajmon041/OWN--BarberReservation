using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Models.Service;
using MediatR;

namespace BarberReservation.Application.Service.Queries.GetServiceById;

public sealed class GetServiceByIdQuery(int id) : IRequest<ServiceDto>, IHasId
{
    public int Id { get; init; } = id;
}