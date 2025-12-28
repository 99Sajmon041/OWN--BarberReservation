using FluentValidation;

namespace BarberReservation.Application.Authorization.Command.ChangePassword;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Stávající heslo je povinné.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Nové heslo je povinné.")
            .MinimumLength(8).WithMessage("Nové heslo musí mít alespoň 8 znaků.")
            .Matches("[0-9]").WithMessage("Nové heslo musí obsahovat alespoň jednu číslici.")
            .Matches("[a-z]").WithMessage("Nové heslo musí obsahovat alespoň jedno malé písmeno.")
            .Matches("[A-Z]").WithMessage("Nové heslo musí obsahovat alespoň jedno velké písmeno.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Potvrzení hesla je povinné.")
            .Equal(x => x.NewPassword).WithMessage("Hesla se musí shodovat.");
    }
}
