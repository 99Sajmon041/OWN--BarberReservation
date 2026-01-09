using BarberReservation.Shared.Enums;

namespace BarberReservation.Shared.Models.Rezervation.Admin;

public sealed class UpdateReservationRequest
{
    public ReservationStatus NewReservationStatus { get; set; }
    public CanceledReason? CanceledReason { get; set; }
}
