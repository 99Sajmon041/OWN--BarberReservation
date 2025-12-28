using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Authorization;

public sealed class ChangePasswordRequest
{
    [Required(ErrorMessage = "Stávající heslo je povinné.")]
    public string OldPassword { get; set; } = default!;

    [Required(ErrorMessage = "Nové heslo je povinné.")]
    [MinLength(8, ErrorMessage = "Nové heslo musí mít alespoň 8 znaků.")]
    [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).*$", ErrorMessage = "Nové heslo musí obsahovat alespoň jednu číslici, jedno malé a jedno velké písmeno.")]
    public string NewPassword { get; set; } = default!;

    [Required(ErrorMessage = "Potvrzení hesla je povinné.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Hesla se musí shodovat.")]
    public string ConfirmPassword { get; set; } = default!;
}
