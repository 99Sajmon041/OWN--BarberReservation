using BarberReservation.Application.Common.Validation.PagingValidation;
using BarberReservation.Application.Common.Validation.SearchValidation;
using BarberReservation.Shared.Enums;
using FluentValidation;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAllReservations;

public sealed class GetAllReservationsQueryValidator : AbstractValidator<GetAllReservationsQuery>
{
    public GetAllReservationsQueryValidator()
    {
        Include(new PagingValidator<GetAllReservationsQuery>());
        Include(new SearchValidator<GetAllReservationsQuery>());

        RuleFor(x => x.HairdresserId)
            .MaximumLength(120).WithMessage("ID kadeřníka může mít maximálně 120 znaků.");

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
            .WithMessage("Konec musí být větší nebo roven začáku zrušení.")
            .When(x => x.CanceledFrom.HasValue && x.CanceledTo.HasValue);

        RuleFor(x => x.Status)
            .Equal(ReservationStatus.Canceled)
            .WithMessage("Zrušeno kým / důvod zrušení lze použít pouze když Status = zrušen.")
            .When(x => x.CanceledBy.HasValue || x.CanceledReason.HasValue || x.CanceledFrom.HasValue || x.CanceledTo.HasValue);
    }
}
