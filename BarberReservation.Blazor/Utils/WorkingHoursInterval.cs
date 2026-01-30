namespace BarberReservation.Blazor.Utils;

public static class WorkingHoursInterval
{
    public static List<TimeOnly> GetHalfHours()
    {
        var result = new List<TimeOnly>();
        var time = new TimeOnly(5, 0);
        var end = new TimeOnly(22, 0);

        while (time <= end)
        {
            result.Add(time);
            time = time.AddMinutes(30);
        }

        return result;
    }
}
