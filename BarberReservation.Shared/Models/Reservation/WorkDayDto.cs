namespace BarberReservation.Shared.Models.Reservation;

public sealed class WorkDayDto
{
    public DateTime Date { get; }
    public string DayLabel { get; }
    public string DateLabel { get; }
    public List<SlotDto> Slots { get; set; } = new();

    public WorkDayDto(DateTime date)
    {
        Date = date;
        DayLabel = date.DayOfWeek switch
        {
            DayOfWeek.Monday => "PO",
            DayOfWeek.Tuesday => "ÚT",
            DayOfWeek.Wednesday => "ST",
            DayOfWeek.Thursday => "ČT",
            DayOfWeek.Friday => "PÁ",
            _ => date.ToString("ddd").ToUpperInvariant()
        };
        DateLabel = date.ToString("dd.MM.yyyy");
    }
}
