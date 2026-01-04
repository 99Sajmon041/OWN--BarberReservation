using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Models.HairdresserService;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetHairdresserService;

public sealed class GetHairdresserServiceByIdQueryQuery(int id) : IRequest<HairdresserServiceDto>, IHasId
{
    public int Id { get; init; } = id;
}
