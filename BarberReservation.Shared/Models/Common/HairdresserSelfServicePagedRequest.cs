using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Common;

public sealed class HairdresserSelfServicePagedRequest : PagedRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "ID služby musí být větší než 0.")]
    public int? ServiceId { get; set; }
}
