using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Service;
using MediatR;

namespace BarberReservation.Application.Service.Queries.GetAllServices;

public sealed class GetAllServicesQuery : IRequest<PagedResult<ServiceDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public bool? IsActive { get; set; }
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool Desc { get; set; } = false;
}
