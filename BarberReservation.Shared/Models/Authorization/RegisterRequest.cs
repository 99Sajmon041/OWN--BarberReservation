using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Authorization;

public sealed class RegisterRequest
{
    [Required(ErrorMessage = "Jméno je povinné.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Délka jména musí být v rozmezí 2 - 20 znaků.")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessage = "Příjmení je povinné.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Délka příjmení musí být v rozmezí 2 - 20 znaků.")]
    public string LastName { get; set; } = default!;

    [Required(ErrorMessage = "E-mail je povinný.")]
    [EmailAddress(ErrorMessage = "Zadejte e-mail ve správném formátu.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Heslo je povinné.")]
    [MinLength(8, ErrorMessage = "Heslo musí mít alespoň 8 znaků.")]
    [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).*$", ErrorMessage = "Heslo musí obsahovat alespoň jednu číslici, jedno malé a jedno velké písmeno.")]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "Telefon je povinný.")]
    [RegularExpression(@"^\+?[1-9]\d{7,14}$", ErrorMessage = "Telefon musí být ve formátu +420123456789 (8–15 číslic, bez mezer, pomlček a závorek).")]
    public string PhoneNumber { get; set; } = default!;
}
