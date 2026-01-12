namespace BarberReservation.Application.Common.DateHelpers;

public static class ReservationDateHelper
{
    private const int WeeksAhead = 3;
    public static DateOnly GetMaxReservationDate()
    {
        var now = DateTime.UtcNow;

        var daysToFriday = ((int)DayOfWeek.Friday - (int)now.DayOfWeek + 7) % 7;

        var upcomingFriday = now.Date.AddDays(daysToFriday);
        var maxDate = upcomingFriday.AddDays(WeeksAhead * 7);

        return DateOnly.FromDateTime(maxDate);
    }
}