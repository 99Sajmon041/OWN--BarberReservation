using BarberReservation.Shared.Enums;
using System.Net.NetworkInformation;

namespace BarberReservation.Shared.Helpers;

public static class Helper
{
    public static readonly (string Value, string Text)[] RoleOptions =
    [
        (nameof(UserRoles.Admin), "Admin"),
        (nameof(UserRoles.Hairdresser), "Kadeřník"),
        (nameof(UserRoles.Customer), "Zákazník")
    ];

    public static readonly HashSet<string> AllowedRoleValues =
        RoleOptions.Select(x => x.Value).ToHashSet(StringComparer.OrdinalIgnoreCase);

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
        string? name = role switch
        {
            nameof(UserRoles.Admin) => "Administrátor",
            nameof(UserRoles.Hairdresser) => "Kadeřník",
            nameof(UserRoles.Customer) => "Zákazník",
            _ => "Role nepřiřazena"
        };

        return name;
    }

    public static string GetCzechCanceledReason(string reason)
    {
        return reason switch
        {
            nameof(CanceledReason.CustomerSick) => "Nemoc zákazníka",
            nameof(CanceledReason.CustomerPersonal) => "Osobní důvody zákazníka",
            nameof(CanceledReason.CustomerRequest) => "Požadavek zákazníka",
            nameof(CanceledReason.HairdresserSick) => "Nemoc kadeřníka",
            nameof(CanceledReason.HairdresserUnavailable) => "Nedostupnost kadeřníka",
            nameof(CanceledReason.Other) => "Jiný důvod",
            _ => "Neznámý důvod"
        };
    }
}
