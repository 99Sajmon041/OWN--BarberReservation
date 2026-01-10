using BarberReservation.Shared.Enums;

namespace BarberReservation.Application.Reservation.Common;

public interface IReservationStatusUpdate
{
    public ReservationStatus NewReservationStatus { get; }
    public CanceledReason? CanceledReason { get; }
}
