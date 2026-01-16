namespace BarberReservation.Shared.Models.TimeOff.Common;

public class TimeOffBaseDto
{
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public string Reason { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
