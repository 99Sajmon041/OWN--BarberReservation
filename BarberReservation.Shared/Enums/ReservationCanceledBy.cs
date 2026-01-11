using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Enums;

public enum ReservationCanceledBy
{
    [Display(Name = "Zákazník")]
    Customer = 0,

    [Display(Name = "Kadeřník")]
    Hairdresser = 1,

    [Display(Name = "Administrátor")]
    Admin = 2
}