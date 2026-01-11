using BarberReservation.Shared.Models.Reservation.Common;

namespace BarberReservation.Shared.Models.Reservation.Self;

public sealed class CreateSelfReservationRequest : CreateReservationBaseRequest 
{
    public string HairdresserId { get; set; } = default!;
}
