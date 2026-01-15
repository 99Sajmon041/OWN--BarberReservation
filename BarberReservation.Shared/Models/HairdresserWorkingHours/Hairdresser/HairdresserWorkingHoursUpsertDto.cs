using BarberReservation.Shared.Models.HairdresserWorkingHours.Common;

namespace BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;

public sealed class HairdresserWorkingHoursUpsertDto
{
    public IReadOnlyCollection<WorkingHoursDto> DaysOfWorkingWeek { get; set; } = [];
}
