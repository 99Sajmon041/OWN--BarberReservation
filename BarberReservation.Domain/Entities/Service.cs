using BarberReservation.Domain.Interfaces;

namespace BarberReservation.Domain.Entities;

public sealed class Service : IActivatable
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsActive { get; set; }
    public ICollection<HairdresserService> HairdresserServices { get; set; } = [];
}
