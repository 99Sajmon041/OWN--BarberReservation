using BarberReservation.Application.Common.PagedSettings;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService.Common;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetAllAdminHairdressersServices;

public sealed class GetAllAdminHairdressersServicesQuery : PagedApiRequest, IRequest<PagedResult<HairdresserServiceDto>>
{
    public bool? IsActive { get; set; }
    public string? HairdresserId { get; init; }
    public int? ServiceId { get; init; }
}
