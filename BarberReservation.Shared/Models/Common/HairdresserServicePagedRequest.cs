namespace BarberReservation.Shared.Models.Common;

public sealed class HairdresserServicePagedRequest : PagedRequest
{
    public string? HairdresserId { get; set; }
    public int? ServiceId { get; set; }
}
