namespace BarberReservation.Shared.Models.HairdresserService;

public sealed class HairdresserServiceDto
{
    public int Id { get; set; }
    public string HairdresserName { get; set; } = default!;
    public string HairdresserId { get; set; } = default!;
    public string ServiceName { get; set; } = default!;
    public int ServiceId { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
