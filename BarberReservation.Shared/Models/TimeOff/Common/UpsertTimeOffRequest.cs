using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.TimeOff.Common;

public class UpsertTimeOffRequest
{
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }

    [StringLength(120, MinimumLength = 5, ErrorMessage = "Důvod volna musí být v rozmezí 5 - 120 znaků.")]
    public string Reason { get; set; } = default!;
}
