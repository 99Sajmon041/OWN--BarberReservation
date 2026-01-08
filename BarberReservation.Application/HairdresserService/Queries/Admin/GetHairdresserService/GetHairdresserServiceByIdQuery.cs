using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Models.HairdresserService.Common;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetHairdresserService;

public sealed class GetHairdresserServiceByIdQuery(int id) : IRequest<HairdresserServiceDto>, IHasId
{
    public int Id { get; init; } = id;
}
