using BarberReservation.Application.Reservation.Validation;
using FluentValidation;
namespace BarberReservation.Application.Reservation.Queries.Admin.GetAllAdminReservations;

public sealed class GetAllAdminReservationsQueryValidator : ReservationListQueryValidatorBase<GetAllAdminReservationsQuery>
{
    public GetAllAdminReservationsQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .MaximumLength(120).WithMessage("ID zákazníka může mít maximálně 120 znaků.");
    }
}
