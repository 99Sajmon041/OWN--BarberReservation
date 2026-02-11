using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Reservation;

public class CreateReservationRequest
{
    public int ServiceId { get; set; }
    public string? HairdresserId { get; set; }
    public string? CustomerId { get; set; }
    public DateTime StartAt { get; set; }

    [Required(ErrorMessage = "Celé jméno je povinné.")]
    [StringLength(41, MinimumLength = 4, ErrorMessage = "Délka celého jména musí být v rozmezí 4 - 41 znaků.")]
    public string CustomerName { get; set; } = default!;

    [Required(ErrorMessage = "E-mail je povinný.")]
    [EmailAddress(ErrorMessage = "Zadejte e-mail ve správném formátu.")]
    public string CustomerEmail { get; set; } = default!;

    [Required(ErrorMessage = "Telefon je povinný.")]
    [RegularExpression(@"^\+?[1-9]\d{7,14}$", ErrorMessage = "Telefon musí být ve formátu +420123456789 (8–15 číslic, bez mezer, pomlček a závorek).")]
    public string CustomerPhone { get; set; } = default!;
}
