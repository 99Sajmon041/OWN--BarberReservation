using BarberReservation.Application.Common.PagedSettings;
using BarberReservation.Application.Common.Security;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService.Common;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Queries.Self.GetAllSelfHairdressersServices;

public sealed class GetAllSelfHairdressersServicesQuery : PagedApiRequest, IRequireActiveUser, IRequest<PagedResult<HairdresserServiceDto>>
{
    public bool? IsActive { get; init; }
    public int? ServiceId { get; init; }
}
