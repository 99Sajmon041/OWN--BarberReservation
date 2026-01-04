using FluentValidation;

namespace BarberReservation.Application.HairdresserService.Commands.Self.CreateSelfHairdresserService;

public sealed class CreateSelfHairdresserServiceCommandValidator : AbstractValidator<CreateSelfHairdresserServiceCommand>
{
    public CreateSelfHairdresserServiceCommandValidator()
    {

        RuleFor(x => x.ServiceId)
            .GreaterThan(0)
            .WithMessage("ID služby musí být větší než 0.");

        RuleFor(x => x.DurationMinutes)
            .InclusiveBetween(10, 100)
            .WithMessage("Služba musí trvat v rozmezí 10 - 100 minut.");

        RuleFor(x => x.Price)
            .InclusiveBetween(100, 5000)
            .WithMessage("Cena musí být v rozmezí 100 - 5 000,- Kč.");
    }
}
