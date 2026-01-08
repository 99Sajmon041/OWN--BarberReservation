using BarberReservation.Shared.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.HairdresserService.Self;

public sealed class HairdresserSelfServicePagedRequest : PagedRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "ID služby musí být větší než 0.")]
    public int? ServiceId { get; set; }
    public bool? IsActive { get; set; }
}
