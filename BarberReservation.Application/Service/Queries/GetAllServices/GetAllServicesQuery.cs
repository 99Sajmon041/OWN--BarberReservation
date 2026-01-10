using BarberReservation.Application.Common.PagedSettings;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Service;
using MediatR;

namespace BarberReservation.Application.Service.Queries.GetAllServices;

public sealed class GetAllServicesQuery : PagedApiRequest, IRequest<PagedResult<ServiceDto>>
{
    public bool? IsActive { get; init; }
}
