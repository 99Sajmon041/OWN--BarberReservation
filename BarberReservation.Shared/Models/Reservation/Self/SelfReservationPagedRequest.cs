using BarberReservation.Shared.Models.Reservation.Common;
using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Reservation.Self;

public sealed class SelfReservationPagedRequest : ReservationPagedBaseRequest
{
    [StringLength(120, ErrorMessage = "ID kadeřníka může mít maximálně 120 znaků.")]
    public string? HairdresserId { get; set; }
}
