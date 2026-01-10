using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;

namespace BarberReservation.Shared.Models.Rezervation.Common;

public class ReservationPagedBaseRequest : PagedRequest
{
    public ReservationStatus? Status { get; set; }
    public ReservationCanceledBy? CanceledBy { get; set; }
    public CanceledReason? CanceledReason { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public DateTime? StartFrom { get; set; }
    public DateTime? StartTo { get; set; }
    public DateTime? CanceledFrom { get; set; }
    public DateTime? CanceledTo { get; set; }
}
