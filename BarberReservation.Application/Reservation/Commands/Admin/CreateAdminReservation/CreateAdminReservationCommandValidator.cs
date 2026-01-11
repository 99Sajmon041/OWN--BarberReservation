using BarberReservation.Application.Reservation.Validation;
using FluentValidation;

namespace BarberReservation.Application.Reservation.Commands.Admin.CreateAdminReservation;

public sealed class CreateAdminReservationCommandValidator : ReservationCreateValidatorBase<CreateAdminReservationCommand>
{
    public CreateAdminReservationCommandValidator()
    {
        RuleFor(x => x.HairdresserId)
            .NotEmpty().WithMessage("ID kadeřníka je povinné.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("ID zákazníka nesmí být prázdné.")
            .When(x => x.CustomerId is not null);
    }
}
