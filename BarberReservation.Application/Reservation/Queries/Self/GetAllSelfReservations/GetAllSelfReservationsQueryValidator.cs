using BarberReservation.Application.Reservation.Validation;
using FluentValidation;

namespace BarberReservation.Application.Reservation.Queries.Self.GetAllSelfReservations;

public sealed class GetAllSelfReservationsQueryValidator : ReservationListQueryValidatorBase<GetAllSelfReservationsQuery>
{
    public GetAllSelfReservationsQueryValidator()
    {
        RuleFor(x => x.HairdresserId)
            .MaximumLength(120)
            .WithMessage("ID kadeřníka může mít maximálně 120 znaků.");
    }
}
