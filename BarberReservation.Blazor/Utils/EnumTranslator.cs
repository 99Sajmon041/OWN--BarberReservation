using BarberReservation.Shared.Enums;

namespace BarberReservation.Blazor.Utils;

public static class EnumTranslator
{
    public static string GetCzechreservationStatus(ReservationStatus status)
    {
        return status switch
        {
            ReservationStatus.Booked => "Rezervováno",
            ReservationStatus.Canceled => "Zrušeno",
            ReservationStatus.NoShow => "Nedostaveno",
            ReservationStatus.Paid => "Zaplaceno",
            _ => "Neznámý stav"
        };
    }
}
