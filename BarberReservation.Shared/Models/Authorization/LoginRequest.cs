using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Authorization;

public sealed class LoginRequest
{
    [Required(ErrorMessage = "E-mail je povinný.")]
    [EmailAddress(ErrorMessage = "Zadejte platnou e-mailovou adresu.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Heslo je povinné.")]
    [MinLength(8, ErrorMessage = "Heslo musí mít alespoň 8 znaků.")]
    public string Password { get; set; } = default!;
    public bool RememberMe { get; set; }
}
