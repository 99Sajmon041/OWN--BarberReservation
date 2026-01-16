using BarberReservation.Shared.Models.TimeOff.Common;

namespace BarberReservation.Shared.Models.TimeOff.Admin;

public sealed class AdminHairdresserTimeOffDto : TimeOffBaseDto
{
    public string HairdresserId { get; set; } = default!;
    public string HairdresserName { get; set; } = default!;
}
