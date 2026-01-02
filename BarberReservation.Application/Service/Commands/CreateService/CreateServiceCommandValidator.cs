using BarberReservation.Application.Service.Validation;
using FluentValidation;

namespace BarberReservation.Application.Service.Commands.CreateService;

public sealed class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        ServiceUpsertRules.Apply(
            RuleFor(x => x.Name),
            RuleFor(x => x.Description)
        );
    }
}
