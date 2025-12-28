using FluentValidation;

namespace BarberReservation.Application.Authorization.Command.ResetPassword;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail je povinný.")
            .EmailAddress().WithMessage("Zadejte e-mail ve správném formátu.");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token je povinný.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Heslo je povinné.")
            .MinimumLength(8).WithMessage("Nové heslo musí mít alespoň 8 znaků.")
            .Matches("[0-9]").WithMessage("Nové heslo musí obsahovat alespoň jednu číslici.")
            .Matches("[a-z]").WithMessage("Nové heslo musí obsahovat alespoň jedno malé písmeno.")
            .Matches("[A-Z]").WithMessage("Nové heslo musí obsahovat alespoň jedno velké písmeno.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Potvrzení hesla je povinné.")
            .Equal(x => x.NewPassword).WithMessage("Hesla se musí shodovat.");
    }
}
