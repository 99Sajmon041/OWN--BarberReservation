using BarberReservation.Shared.Models.HairdresserWorkingHours.Common;

namespace BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;

public sealed class HairdresserWorkingHoursDto 
{
    public IReadOnlyList<WorkingHoursDto> WorkingHours { get; set; } = [];
    public DateOnly EffectiveFrom { get; set; }
}
