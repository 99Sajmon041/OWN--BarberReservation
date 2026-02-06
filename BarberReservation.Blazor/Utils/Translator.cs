using BarberReservation.Shared.Enums;

namespace BarberReservation.Blazor.Utils;

public static class Translator
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

    public static string GetCzechRoleName(string role)
    {
        return role switch
        { 
            "Admin" => "Administrátor",
            "Hairdresser" => "Kadeřník",
            "Customer" => "Zákazník",
            _ => "Neznámá role"
        };
    }
}
