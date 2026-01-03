using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetAllHairdresserServices;

public sealed class GetAllHairdresserServicesQuery : IRequest<PagedResult<HairdresserServiceDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public bool? IsActive { get; set; } 
    public string? HairdresserId { get; set; }
    public int? ServiceId { get; set; }
    public string? SortBy { get; set; }
    public bool Desc { get; set; } = false;
}
