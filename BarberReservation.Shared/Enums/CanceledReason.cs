using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Enums;

public enum CanceledReason
{
    [Display(Name = "Nemoc zákazníka")]
    CustomerSick = 0,

    [Display(Name = "Osobní důvody zákazníka")]
    CustomerPersonal = 1,

    [Display(Name = "Požadavek zákazníka")]
    CustomerRequest = 2,

    [Display(Name = "Nemoc kadeřníka")]
    HairdresserSick = 3,

    [Display(Name = "Nedostupnost kadeřníka")]
    HairdresserUnavailable = 4,

    [Display(Name = "Jiný důvod")]
    Other = 5
}
