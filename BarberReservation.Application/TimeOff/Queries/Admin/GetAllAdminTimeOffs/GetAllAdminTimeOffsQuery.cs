using BarberReservation.Application.Common.PagedSettings;
using BarberReservation.Application.TimeOff.Common;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.TimeOff;
using MediatR;

namespace BarberReservation.Application.TimeOff.Queries.Admin.GetAllAdminTimeOffs;

public sealed class GetAllAdminTimeOffsQuery : PagedApiRequest, ITimeOffListFilter, IRequest<PagedResult<HairdresserTimeOffDto>>
{
    public string? HairdresserId { get; init; }
    public int? Year { get; init; }
    public int? Month { get; init; }
}
