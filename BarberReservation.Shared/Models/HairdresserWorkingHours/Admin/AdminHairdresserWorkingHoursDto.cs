using BarberReservation.Shared.Models.HairdresserWorkingHours.Common;

namespace BarberReservation.Shared.Models.HairdresserWorkingHours.Admin;

public sealed class AdminHairdresserWorkingHoursDto : WorkingHoursDto
{
    public string HairdresserName { get; set; } = default!;
}
