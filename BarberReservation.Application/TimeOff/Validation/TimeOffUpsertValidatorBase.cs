using BarberReservation.Application.TimeOff.Common;
using FluentValidation;

namespace BarberReservation.Application.TimeOff.Validation;

public abstract class TimeOffUpsertValidatorBase<T> : AbstractValidator<T> where T : class, ITimeOffUpsert
{
    protected TimeOffUpsertValidatorBase()
    {
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Důvod volna je povinný.")
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Důvod volna nesmí obsahovat pouze bílé znaky.")
            .Length(5, 120).WithMessage("Důvod volna musí být v rozmezí 5 - 120 znaků.");

        RuleFor(x => x.StartAt)
            .Must(x => x >= DateTime.UtcNow.AddHours(1))
            .WithMessage("Datum a čas volna nesmí být v minulosti a musí začínat 1 hodinu od aktuálního času. Hodinu předem nelze upravovat.");

        RuleFor(x => x.EndAt)
            .GreaterThan(x => x.StartAt).WithMessage("Konec volna musí být po začátku.");
    }
}
