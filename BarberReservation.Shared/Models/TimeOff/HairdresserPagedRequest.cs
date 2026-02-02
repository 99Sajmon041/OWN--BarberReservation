using BarberReservation.Shared.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.TimeOff;

public sealed class HairdresserPagedRequest : PagedRequest 
{
    [StringLength(120, ErrorMessage = "ID kadeřníka může mít maximálně 120 znaků.")]
    public string? HairdresserId { get; set; }

    [Range(2000, 2100)]
    public int? Year { get; set; }

    [Range(1, 12)]
    public int? Month { get; set; }
}
