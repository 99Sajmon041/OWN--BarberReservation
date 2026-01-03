using BarberReservation.Application.HairdresserService.Commands.Admin.NewFolder;
using FluentValidation;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.DeactivateHairdresserService;

public sealed class DeactivateHairdresserServiceCommandValidator : AbstractValidator<DeactivateHairdresserServiceCommand>
{
    public DeactivateHairdresserServiceCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID je povinné.");
    }
}
