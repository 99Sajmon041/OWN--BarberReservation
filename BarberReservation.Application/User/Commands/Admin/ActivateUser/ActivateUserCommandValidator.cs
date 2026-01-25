using FluentValidation;

namespace BarberReservation.Application.User.Commands.Admin.ActivateUser;

public sealed class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
{
    public ActivateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID je povinné.");
    }
}
