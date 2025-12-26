namespace BarberReservation.Domain.Entities;

public sealed class HairdresserService
{
    public int Id { get; set; }
    public ApplicationUser Hairdresser { get; set; } = default!;
    public string HairdresserId { get; set; } = default!;
    public Service Service { get; set; } = default!;
    public int ServiceId { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = [];
}
