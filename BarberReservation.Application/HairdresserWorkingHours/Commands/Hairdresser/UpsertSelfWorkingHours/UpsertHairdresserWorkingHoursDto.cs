namespace BarberReservation.Application.HairdresserWorkingHours.Commands.Hairdresser.UpsertSelfWorkingHours;

public sealed class UpsertHairdresserWorkingHoursDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public bool IsWorkingDay { get; set; }
    public TimeOnly WorkFrom { get; set; }
    public TimeOnly WorkTo { get; set; }
}
