using BarberReservation.Shared.Enums;

namespace BarberReservation.Shared.Models.Reservation.Common;

public sealed class UpdateReservationRequest
{
    public ReservationStatus NewReservationStatus { get; set; }
    public CanceledReason? CanceledReason { get; set; }
}
