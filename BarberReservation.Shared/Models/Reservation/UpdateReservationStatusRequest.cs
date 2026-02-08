using BarberReservation.Shared.Enums;

namespace BarberReservation.Shared.Models.Reservation;

public sealed class UpdateReservationStatusRequest
{
    public ReservationStatus NewReservationStatus { get; set; }
    public CanceledReason? CanceledReason { get; set; }
}
