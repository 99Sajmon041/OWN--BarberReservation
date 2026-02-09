using BarberReservation.Application.Common.Validation.PagingValidation;
using BarberReservation.Application.Common.Validation.SearchValidation;
using BarberReservation.Application.Reservation.Common.Interfaces;
using BarberReservation.Shared.Enums;
using FluentValidation;

namespace BarberReservation.Application.Reservation.Validation;

public abstract class ReservationListQueryValidatorBase<T> : AbstractValidator<T> where T : class, IReservationListFilter, IHasSearch, IHasPaging
{
    protected ReservationListQueryValidatorBase()
    {
        Include(new PagingValidator<T>());
        Include(new SearchValidator<T>());

        RuleFor(x => x.ServiceId)
            .GreaterThan(0)
            .When(x => x.ServiceId is not null);

        RuleFor(x => x.HairdresserId)
            .MaximumLength(120)
            .WithMessage("ID kadeřníka může mít maximálně 120 znaků.");

        RuleFor(x => x.CreatedTo)
            .GreaterThanOrEqualTo(x => x.CreatedFrom!.Value)
            .WithMessage("Vytvořeno do musí být větší nebo rovno vytvořeno od.")
            .When(x => x.CreatedFrom.HasValue && x.CreatedTo.HasValue);

        RuleFor(x => x.StartTo)
            .GreaterThanOrEqualTo(x => x.StartFrom!.Value)
            .WithMessage("Konec musí být větší nebo rovno začátku služby.")
            .When(x => x.StartFrom.HasValue && x.StartTo.HasValue);

        RuleFor(x => x.CanceledTo)
            .GreaterThanOrEqualTo(x => x.CanceledFrom!.Value)
            .WithMessage("Konec musí být větší nebo roven začátku zrušení.")
            .When(x => x.CanceledFrom.HasValue && x.CanceledTo.HasValue);

        RuleFor(x => x.Status)
            .NotNull().WithMessage("Pro filtraci dle zrušení je nutné vyplnit Status = zrušen.")
            .Must(s => s == ReservationStatus.Canceled)
            .WithMessage("Zrušeno kým / důvod zrušení lze použít pouze když Status = zrušen.")
            .When(x => x.CanceledBy.HasValue || x.CanceledReason.HasValue || x.CanceledFrom.HasValue || x.CanceledTo.HasValue);
    }
}
