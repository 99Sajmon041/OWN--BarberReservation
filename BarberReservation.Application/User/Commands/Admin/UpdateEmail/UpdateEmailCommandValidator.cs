using FluentValidation;

namespace BarberReservation.Application.User.Commands.Admin.UpdateEmail;

public sealed class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
{
    public UpdateEmailCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID je povinné.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail je povinný.")
            .EmailAddress().WithMessage("Zadejte e-mail ve správném formátu.");
    }
}
