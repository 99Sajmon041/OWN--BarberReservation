using BarberReservation.Shared.Models.Rezervation.Common;

namespace BarberReservation.Shared.Models.Rezervation.Admin;

public sealed class CreateAdminReservationRequest : CreateReservationBaseRequest
{
    public string HairdresserId { get; set; } = default!;
}
