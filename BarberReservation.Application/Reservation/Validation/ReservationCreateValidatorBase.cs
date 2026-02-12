using BarberReservation.Application.Reservation.Common.Interfaces;
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
            .Must(startAt => startAt > DateTime.Now)
            .WithMessage("Datum a čas rezervace musí být v budoucnosti.");

        RuleFor(x => x.ServiceId)
            .GreaterThan(0).WithMessage("ID služby musí být větší než nula.");
    }
}
