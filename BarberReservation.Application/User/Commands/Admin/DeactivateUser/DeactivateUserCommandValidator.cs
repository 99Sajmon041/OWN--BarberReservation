using FluentValidation;

namespace BarberReservation.Application.User.Commands.Admin.DeactivateUser;

public sealed class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
{
    public DeactivateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID je povinné.");
    }
}
