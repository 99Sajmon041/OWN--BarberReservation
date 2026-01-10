using BarberReservation.Application.Common.PagedSettings;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.User.Common;
using MediatR;

namespace BarberReservation.Application.User.Queries.Admin.GetAllUsers;

public sealed class GetAllUsersQuery : PagedApiRequest, IRequest<PagedResult<UserDto>>
{
    public bool? IsActive { get; init; }
}
