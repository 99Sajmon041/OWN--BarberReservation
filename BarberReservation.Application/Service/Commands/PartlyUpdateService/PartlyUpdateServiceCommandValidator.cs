using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Application.Service.Validation;

namespace BarberReservation.Application.Service.Commands.PartlyUpdateService;

public sealed class PartlyUpdateServiceCommandValidator : IdValidator<PartlyUpdateServiceCommand>
{
    public PartlyUpdateServiceCommandValidator()
    {
        ServiceUpsertRules.Apply(
            RuleFor(x => x.Name),
            RuleFor(x => x.Description)
        );
    }
}
