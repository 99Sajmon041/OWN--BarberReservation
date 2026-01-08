using BarberReservation.Application.Common.PagedSettings;
using BarberReservation.Application.Common.Validation.SearchValidation;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService.Common;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetAllHairdresserServices;

public sealed class GetAllHairdressersServicesQuery : PagedApiRequest, IHasSearch, IRequest<PagedResult<HairdresserServiceDto>>
{
    public bool? IsActive { get; set; } 
    public string? HairdresserId { get; set; }
    public string? Search { get; set; }
    public int? ServiceId { get; set; }
    public string? SortBy { get; set; }
    public bool Desc { get; set; } = false;
}
