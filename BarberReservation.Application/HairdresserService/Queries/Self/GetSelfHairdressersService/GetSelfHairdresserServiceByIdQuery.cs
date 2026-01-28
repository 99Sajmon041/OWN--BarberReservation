using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Models.HairdresserService;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Queries.Self.GetSelfHairdressersService;

public sealed class GetSelfHairdresserServiceByIdQuery(int id) : IRequest<HairdresserServiceDto>, IHasId, IRequireActiveUser
{
    public int Id { get; } = id;
}
