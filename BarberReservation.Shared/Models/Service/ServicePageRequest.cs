using BarberReservation.Shared.Models.Common;

namespace BarberReservation.Shared.Models.Service;

public sealed class ServicePageRequest : PagedRequest
{
    public bool? IsActive { get; set; }
}
