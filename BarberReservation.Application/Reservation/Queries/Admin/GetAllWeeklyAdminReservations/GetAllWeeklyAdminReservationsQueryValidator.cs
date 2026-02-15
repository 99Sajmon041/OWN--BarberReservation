using BarberReservation.Application.Reservation.Queries.Self;
using FluentValidation;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAllWeeklyAdminReservations;

public sealed class GetAllWeeklyAdminReservationsQueryValidator : ReservationsByWeekValidator<GetAllWeeklyAdminReservationsQuery>
{
    public GetAllWeeklyAdminReservationsQueryValidator()
    {
        RuleFor(x => x.HairdresserId)
            .NotEmpty().WithMessage("ID kadeřníka nesmí být prázdné.")
            .MaximumLength(120).WithMessage("ID kadeřníka musí být maximálně do 120 znaků.");
    }
}
