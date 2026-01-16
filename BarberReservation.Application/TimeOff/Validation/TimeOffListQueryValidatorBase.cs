using BarberReservation.Application.Common.Validation.PagingValidation;
using BarberReservation.Application.Common.Validation.SearchValidation;
using BarberReservation.Application.TimeOff.Common;
using FluentValidation;

namespace BarberReservation.Application.TimeOff.Validation;

public class TimeOffListQueryValidatorBase<T> : AbstractValidator<T> where T : class, ITimeOffListFilter, IHasSearch, IHasPaging
{
    public TimeOffListQueryValidatorBase()
    {
        Include(new SearchValidator<T>());
        Include(new PagingValidator<T>());

        RuleFor(x => x.Year)
            .NotNull()
            .WithMessage("Rok je povinný, pokud zadáte měsíc.")
            .When(x => x.Month != null);

        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100)
            .WithMessage("Rok má povolené rozmezí: 2000 - 2100")
            .When(x => x.Year != null);

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12)
            .WithMessage("Měsíc má povolené rozmezí: 1 - 12")
            .When(x => x.Month != null);
    }
}
