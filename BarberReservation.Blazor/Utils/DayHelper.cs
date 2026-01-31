namespace BarberReservation.Blazor.Utils;

public static class DayHelper
{
    public static string GetDayName(DayOfWeek day) => day switch
    {
        DayOfWeek.Monday => "Pondělí",
        DayOfWeek.Tuesday => "Úterý",
        DayOfWeek.Wednesday => "Středa",
        DayOfWeek.Thursday => "Čtvrtek",
        DayOfWeek.Friday => "Pátek",
        _ => day.ToString()
    };
}
