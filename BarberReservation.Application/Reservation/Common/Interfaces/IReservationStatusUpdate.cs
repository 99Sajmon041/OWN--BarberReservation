using BarberReservation.Shared.Enums;

namespace BarberReservation.Application.Reservation.Common.Interfaces;

public interface IReservationStatusUpdate
{
    public ReservationStatus NewReservationStatus { get; }
    public CanceledReason? CanceledReason { get; }
}
