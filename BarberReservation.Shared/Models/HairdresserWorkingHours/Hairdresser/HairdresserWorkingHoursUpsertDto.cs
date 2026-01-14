namespace BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;

public sealed class HairdresserWorkingHoursUpsertDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public bool IsWorkingDay { get; set; }
    public TimeOnly WorkFrom { get; set; }
    public TimeOnly WorkTo { get; set; }
}
