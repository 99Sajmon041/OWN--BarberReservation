using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Authorization;

public sealed class ResetPasswordRequest
{
    [Required(ErrorMessage = "E-mail je povinný.")]
    [EmailAddress(ErrorMessage = "Zadejte e-mail ve správném formátu.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Token je povinný.")]
    public string Token { get; set; } = default!;

    [Required(ErrorMessage = "Heslo je povinné.")]
    [MinLength(8, ErrorMessage = "Heslo musí mít alespoň 8 znaků.")]
    [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).*$", ErrorMessage = "Heslo musí obsahovat alespoň jednu číslici, jedno malé a jedno velké písmeno.")]
    public string NewPassword { get; set; } = default!;

    [Required(ErrorMessage = "Potvrzení hesla je povinné.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Hesla se musí shodovat.")]
    public string ConfirmPassword { get; set; } = default!;
}
