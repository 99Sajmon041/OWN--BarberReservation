namespace BarberReservation.Shared.Models.TimeOff;

public sealed class HairdresserTimeOffDto
{
    public int Id { get; set; }
    public string? HairdresserId { get; set; }
    public string? HairdresserName { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public string Reason { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
