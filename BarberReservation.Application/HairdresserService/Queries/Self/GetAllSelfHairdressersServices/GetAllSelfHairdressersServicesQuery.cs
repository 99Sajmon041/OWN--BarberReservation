using BarberReservation.Application.Common.PagedSettings;
using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Common.Validation.SearchValidation;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService.Common;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Queries.Self.GetAllSelfHairdressersServices;

public sealed class GetAllSelfHairdressersServicesQuery : PagedApiRequest, IHasSearch, IRequireActiveUser, IRequest<PagedResult<HairdresserServiceDto>>
{
    public bool? IsActive { get; init; }
    public int? ServiceId { get; init; }
    public string? Search { get; init; }
    public string? SortBy { get; init; }
    public bool Desc { get; init; } = false;
}
