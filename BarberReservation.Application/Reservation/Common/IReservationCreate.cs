namespace BarberReservation.Application.Reservation.Common;

public interface IReservationCreate
{
    public int HairDresserServiceId { get; }
    public DateTime StartAt { get; }
    public string? CustomerId { get; }
    public string CustomerName { get; }
    public string CustomerEmail { get; }
    public string CustomerPhone { get; }
}
