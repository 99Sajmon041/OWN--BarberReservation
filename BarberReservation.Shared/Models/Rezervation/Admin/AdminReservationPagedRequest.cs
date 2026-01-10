using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Rezervation.Common;

public sealed class AdminReservationPagedRequest : ReservationPagedBaseRequest
{
    [StringLength(120, ErrorMessage = "ID kadeřníka může mít maximálně 120 znaků.")]
    public string? HairdresserId { get; set; }
}
