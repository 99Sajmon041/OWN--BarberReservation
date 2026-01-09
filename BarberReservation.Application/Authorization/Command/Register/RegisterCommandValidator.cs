using FluentValidation;

namespace BarberReservation.Application.Authorization.Command.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Jméno je povinné.")
            .Length(2, 20).WithMessage("Délka jména musí být v rozmezí 2 - 20 znaků.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Příjmení je povinné.")
            .Length(2, 20).WithMessage("Délka příjmení musí být v rozmezí 2 - 20 znaků.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail je povinný.")
            .EmailAddress().WithMessage("Zadejte E-mail ve správném formátu.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Heslo je povinné.")
            .MinimumLength(8).WithMessage("Nové heslo musí mít alespoň 8 znaků.")
            .Matches("[0-9]").WithMessage("Nové heslo musí obsahovat alespoň jednu číslici.")
            .Matches("[a-z]").WithMessage("Nové heslo musí obsahovat alespoň jedno malé písmeno.")
            .Matches("[A-Z]").WithMessage("Nové heslo musí obsahovat alespoň jedno velké písmeno.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefonní číslo je povinné.")
            .Matches(@"^\+?[1-9]\d{7,14}$")
            .WithMessage("Telefonní číslo musí být ve formátu +420123456789 (8–15 číslic, volitelně s +).");
    }
}
