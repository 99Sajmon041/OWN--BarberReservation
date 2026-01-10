using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Models.HairdresserService.Common;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetAdminHairdresserService;

public sealed class GetAdminHairdresserServiceByIdQuery(int id) : IRequest<HairdresserServiceDto>, IHasId
{
    public int Id { get; init; } = id;
}
