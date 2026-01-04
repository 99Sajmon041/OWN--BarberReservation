using BarberReservation.Application.Common.PagedResultSettings;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.User.Common;
using MediatR;

namespace BarberReservation.Application.User.Queries.Admin.GetAllUsers;

public sealed class GetAllUsersQuery : PagedApiRequest, IRequest<PagedResult<UserDto>>
{
    public bool? IsActive { get; set; }
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool Desc { get; set; } = false;
}
