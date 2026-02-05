using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Reservation.Common;

public class ReservationPagedRequest : PagedRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "ID služby musí být větší než 0.")]
    public int? ServiceId { get; set; }

    [StringLength(120, ErrorMessage = "ID kadeřníka může mít maximálně 120 znaků.")]
    public string? HairdresserId { get; set; }

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
