using BarberReservation.Shared.Enums;

namespace BarberReservation.Shared.Models.Reservation.Common;

public sealed class ReservationDto
{
    public int Id { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public string ServiceName { get; set; } = default!;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public ReservationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public ReservationCanceledBy? CanceledBy { get; set; }
    public CanceledReason? CanceledReason { get; set; }
    public string HairdresserId { get; set; } = default!;
    public string? HairdresserFullName { get; set; }
    public string? CustomerId { get; set; }
    public string? ClientFullName { get; set; }
    public string? ClientEmail { get; set; }
    public string? ClientPhone { get; set; }
}