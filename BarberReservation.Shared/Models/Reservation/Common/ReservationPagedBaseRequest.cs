using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Reservation.Common;

public class ReservationPagedBaseRequest : PagedRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "ID služby musí být větší než 0.")]
    public int? ServiceId { get; set; }
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
