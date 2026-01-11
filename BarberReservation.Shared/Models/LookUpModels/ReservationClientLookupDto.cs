namespace BarberReservation.Shared.Models.LookUpModels;

public sealed class ReservationClientLookUpDto
{
    public string CustomerId { get; set; } = default!;
    public string CustomerName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerPhone { get; set; } = default!;
}
