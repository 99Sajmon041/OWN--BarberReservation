using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Authorization;

public sealed class ForgotPasswordRequest
{
    [Required(ErrorMessage = "E-mail je povinný.")]
    [EmailAddress(ErrorMessage = "Zadejte e-mail ve správném formátu.")]
    public string Email { get; set; } = default!;
}