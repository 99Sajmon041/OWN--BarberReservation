using BarberReservation.Shared.Models.Reservation.Common;

namespace BarberReservation.Shared.Models.Reservation.Admin;

public sealed class CreateAdminReservationRequest : CreateReservationBaseRequest
{
    public string? CustomerId { get; set; }
    public string HairdresserId { get; set; } = default!;
}
