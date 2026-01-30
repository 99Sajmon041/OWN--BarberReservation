namespace BarberReservation.Shared.Models.HairdresserWorkingHours;

public sealed class HairdresserWorkingHoursUpsertDto
{
    public List<WorkingHoursDto> DaysOfWorkingWeek { get; set; } = [];
}
