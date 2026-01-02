using FluentValidation;

namespace BarberReservation.Application.User.Commands.Admin.PartlyUpdateUser;

public sealed class PartlyUpdateUserCommandValidator : AbstractValidator<PartlyUpdateUserCommand>
{
    public PartlyUpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID je povinné.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Jméno je povinné.")
            .Length(2, 20).WithMessage("Délka jména musí být v rozmezí 2 - 20 znaků.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Příjmení je povinné.")
            .Length(2, 20).WithMessage("Délka příjmení musí být v rozmezí 2 - 20 znaků.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefonní číslo je povinné.")
            .Matches(@"^\+?[1-9]\d{7,14}$")
            .WithMessage("Telefonní číslo musí být ve formátu +420123456789 (8–15 číslic, volitelně s +).");
    }
}
