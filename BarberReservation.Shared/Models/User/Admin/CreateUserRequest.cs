using BarberReservation.Shared.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.User.Admin;

public sealed class CreateUserRequest
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

    [Required(ErrorMessage = "Telefon je povinný.")]
    [RegularExpression(@"^\+?[1-9]\d{7,14}$", ErrorMessage = "Telefon musí být ve formátu +420123456789 (8–15 číslic, bez mezer, pomlček a závorek).")]
    public string PhoneNumber { get; set; } = default!;

    [Required(ErrorMessage = "Role je poviná.")]
    public string Role { get; set; } = default!;


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!EnumHelper.AllowedRoleValues.Contains(Role))
            yield return new ValidationResult("Neplatná role,", new[] { nameof(Role) });
    }
}
