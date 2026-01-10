using BarberReservation.Application.Reservation.Validation;
using FluentValidation;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAllAdminReservations;

public sealed class GetAllAdminReservationsQueryValidator : ReservationListQueryValidatorBase<GetAllAdminReservationsQuery>
{
    public GetAllAdminReservationsQueryValidator()
    {
        RuleFor(x => x.HairdresserId)
            .MaximumLength(120)
            .WithMessage("ID kadeřníka může mít maximálně 120 znaků.");
    }
}
