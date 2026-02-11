namespace BarberReservation.Application.Reservation.Common.Interfaces;

public interface IReservationCreate
{
    public int ServiceId { get; }
    public DateTime StartAt { get; }
    public string CustomerName { get; }
    public string CustomerEmail { get; }
    public string CustomerPhone { get; }
}
