namespace BarberReservation.Shared.Models.HairdresserWorkingHours.Common;

public class WorkingHoursDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public bool IsWorkingDay { get; set; }
    public TimeOnly WorkFrom { get; set; }
    public TimeOnly WorkTo { get; set; }
}
