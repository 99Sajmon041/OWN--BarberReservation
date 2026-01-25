using BarberReservation.Shared.Enums;

namespace BarberReservation.Shared.Helpers;

public static class EnumHelper
{
    public static readonly (string Value, string Text)[] RoleOptions =
    [
        (nameof(UserRoles.Admin), "Admin"),
        (nameof(UserRoles.Hairdresser), "Kadeřník"),
        (nameof(UserRoles.Customer), "Zákazník"),
    ];

    public static readonly HashSet<string> AllowedRoleValues =
        RoleOptions.Select(x => x.Value).ToHashSet(StringComparer.OrdinalIgnoreCase);

    public static string GetCzechNameOfRole(string role)
    {
        var name = string.Empty;

        switch (role)
        {
            case nameof(UserRoles.Admin):
                name = "Administrátor";
                break;

            case nameof(UserRoles.Hairdresser):
                name = "Kadeřník";
                break;

            case nameof(UserRoles.Customer):
                name = "Zákazník";
                break;

            default:
                name = "Role nepřiřazena";
                break;
        };

        return name;
    }


    //public static string GetCzechNameOfRole(string role)
    //{
    //    string? name = role switch
    //    {
    //        nameof(UserRoles.Admin) => "Administrátor",
    //        nameof(UserRoles.Hairdresser) => "Kadeřník",
    //        nameof(UserRoles.Customer) => "Zákazník",
    //        _ => "Role nepřiřazena",
    //    };
    //
    //    return name;
    //}
}
