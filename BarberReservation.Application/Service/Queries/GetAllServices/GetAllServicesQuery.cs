using BarberReservation.Application.Common.PagedSettings;
using BarberReservation.Application.Common.Validation.SearchValidation;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Service;
using MediatR;

namespace BarberReservation.Application.Service.Queries.GetAllServices;

public sealed class GetAllServicesQuery : PagedApiRequest, IHasSearch, IRequest<PagedResult<ServiceDto>>
{
    public bool? IsActive { get; set; }
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool Desc { get; set; } = false;
}
