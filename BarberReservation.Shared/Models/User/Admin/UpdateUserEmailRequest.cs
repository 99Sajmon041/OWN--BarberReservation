using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.User.Admin;

public sealed class UpdateUserEmailRequest
{
    [Required(ErrorMessage = "E-mail je povinný.")]
    [EmailAddress(ErrorMessage = "Zadejte e-mail ve správném formátu.")]
    public string Email { get; set; } = default!;
}
