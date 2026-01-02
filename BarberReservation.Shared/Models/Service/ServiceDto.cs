namespace BarberReservation.Shared.Models.Service;

public sealed class ServiceDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public bool IsActive { get; init; }
}
