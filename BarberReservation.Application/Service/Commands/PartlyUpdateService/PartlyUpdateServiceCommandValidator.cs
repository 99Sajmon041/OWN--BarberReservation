using BarberReservation.Application.Service.Validation;
using FluentValidation;

namespace BarberReservation.Application.Service.Commands.PartlyUpdateService;

public sealed class PartlyUpdateServiceCommandValidator : AbstractValidator<PartlyUpdateServiceCommand>
{
    public PartlyUpdateServiceCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID je povinné.");

        ServiceUpsertRules.Apply(
            RuleFor(x => x.Name),
            RuleFor(x => x.Description)
        );
    }
}
