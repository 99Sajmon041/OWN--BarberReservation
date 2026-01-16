using BarberReservation.Application.Common.PagedSettings;
using BarberReservation.Application.Common.Security;
using BarberReservation.Application.TimeOff.Common;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.TimeOff.Hairdresser;
using MediatR;

namespace BarberReservation.Application.TimeOff.Queries.Hairdresser.GetAllSelfTimeOffs;

public sealed class GetAllSelfTimeOffsQuery : PagedApiRequest, IRequireActiveUser, ITimeOffListFilter, IRequest<PagedResult<HairdresserTimeOffDto>>
{
    public int? Year { get; init; }
    public int? Month { get; init; }
}
