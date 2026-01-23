using BarberReservation.Shared.Models.Common;

namespace BarberReservation.Shared.Models.User.Admin;

public sealed class UserPageRequest : PagedRequest
{
    public bool? IsActive { get; set; }
}
