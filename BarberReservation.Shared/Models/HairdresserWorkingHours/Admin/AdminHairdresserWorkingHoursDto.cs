using BarberReservation.Shared.Models.HairdresserWorkingHours.Common;

namespace BarberReservation.Shared.Models.HairdresserWorkingHours.Admin;

public sealed class AdminHairdresserWorkingHoursDto
{
    public IReadOnlyList<WorkingHoursDto> WorkingHours { get; set; } = [];
    public string HairdresserName { get; set; } = default!;
    public DateOnly EffectiveFrom { get; set; }
}
