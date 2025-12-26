namespace BarberReservation.Domain.Entities;

public sealed class HairdresserTimeOff
{
    public int Id { get; set; }
    public ApplicationUser Hairdresser { get; set; } = default!;
    public string HairdresserId { get; set; } = default!;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public string Reason { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
