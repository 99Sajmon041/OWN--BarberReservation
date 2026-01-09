using BarberReservation.Shared.Enums;

namespace BarberReservation.Shared.Models.Rezervation.Admin;

public sealed class AdminReservationDto
{
    public int Id { get; set; }
    public string HairdresserId { get; set; } = default!;
    public string HairdresserFullName { get; set; } = default!;
    public int HairdresserServiceId { get; set; } = default!;
    public string ServiceName { get; set; } = default!;
    public int? DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public ReservationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public ReservationCanceledBy? CanceledBy { get; set; }
    public CanceledReason? CanceledReason { get; set; }
    public string CustomerId { get; set; } = default!;
    public string ClientFullName { get; set; } = default!;
    public string ClientEmail { get; set; } = default!;
    public string ClientPhone { get; set; } = default!;
}
