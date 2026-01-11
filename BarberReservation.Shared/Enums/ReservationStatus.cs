using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Enums;

public enum ReservationStatus
{
    [Display(Name = "Rezervováno")]
    Booked = 0,

    [Display(Name = "Zrušeno")]
    Canceled = 1,

    [Display(Name = "Nedostavil se")]
    NoShow = 2,

    [Display(Name = "Zaplaceno")]
    Paid = 3
}
