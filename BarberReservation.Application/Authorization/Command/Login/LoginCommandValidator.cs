using FluentValidation;

namespace BarberReservation.Application.Authorization.Command.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail je povinný údaj.")
            .EmailAddress().WithMessage("Zadejte platnou e-mailovou adresu.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Heslo je povinný údaj.")
            .MinimumLength(8).WithMessage("Heslo musí mít alespoň 8 znaků.");
    }
}