using BarberReservation.Shared.Enums;

namespace BarberReservation.Domain.Entities;

public sealed class Reservation
{
    public int Id { get; set; }
    public ApplicationUser Hairdresser { get; set; } = default!;
    public string HairdresserId { get; set; } = default!;
    public HairdresserService HairdresserService { get; set; } = default!;
    public int HairdresserServiceId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public ReservationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public ReservationCanceledBy? CanceledBy { get; set; }
    public CanceledReason? CanceledReason  { get; set; }
    public ApplicationUser? Customer { get; set; }
    public string? CustomerId { get; set; }
    public string CustomerName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerPhone { get; set; } = default!;
}
