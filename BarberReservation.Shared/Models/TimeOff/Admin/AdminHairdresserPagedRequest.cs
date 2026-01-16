using BarberReservation.Shared.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.TimeOff.Admin;

public sealed class AdminHairdresserPagedRequest : PagedRequest
{
    [StringLength(120, ErrorMessage = "ID kadeřníka může mít maximálně 120 znaků.")]
    public string? HairdresserId { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
}
