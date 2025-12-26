namespace BarberReservation.Domain.Entities;

public sealed class HairdresserWorkingHours
{
    public int Id { get; set; }
    public ApplicationUser Hairdresser { get; set; } = default!;
    public string HairdresserId { get; set; } = default!;
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly WorkFrom { get; set; }
    public TimeOnly WorkTo { get; set; }
}
