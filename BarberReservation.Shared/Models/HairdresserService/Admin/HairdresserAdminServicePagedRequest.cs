using BarberReservation.Shared.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.HairdresserService.Admin;

public sealed class HairdresserAdminServicePagedRequest : PagedRequest
{
    [StringLength(120, ErrorMessage = "ID kadeřníka může mít maximálně 120 znaků.")]
    public string? HairdresserId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ID služby musí být větší než 0.")]
    public int? ServiceId { get; set; }
    public bool? IsActive { get; set; }
}
