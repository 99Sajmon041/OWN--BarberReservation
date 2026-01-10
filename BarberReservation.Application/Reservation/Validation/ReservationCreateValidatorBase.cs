using BarberReservation.Application.Reservation.Common;
using FluentValidation;

namespace BarberReservation.Application.Reservation.Validation;

public abstract class ReservationCreateValidatorBase<T> : AbstractValidator<T> where T : class, IReservationCreate
{
    protected ReservationCreateValidatorBase()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Celé jméno je povinné.")
            .Length(4, 41).WithMessage("Délka celého jména musí být v rozmezí 4 - 41 znaků.");

        RuleFor(x => x.CustomerEmail)
            .NotEmpty().WithMessage("E-mail je povinný.")
            .EmailAddress().WithMessage("Zadejte e-mail ve správném formátu.");

        RuleFor(x => x.CustomerPhone)
            .NotEmpty().WithMessage("Telefonní číslo je povinné.")
            .Matches(@"^\+?[1-9]\d{7,14}$")
            .WithMessage("Telefonní číslo musí být ve formátu +420123456789 (8–15 číslic, volitelně s +).");
         
        RuleFor(x => x.StartAt)
            .Must((x, startAt) => startAt > DateTime.UtcNow)
            .WithMessage("Datum a čas rezervace musí být v budoucnosti.");

        RuleFor(x => x.HairDresserServiceId)
            .GreaterThan(0).WithMessage("ID služby kadeřníka musí být větší než nula.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("ID zákazníka nesmí být prázdné.")
            .When(x => x.CustomerId is not null);
    }
}
