using BarberReservation.Application.Reservation.Validation;
using FluentValidation;

namespace BarberReservation.Application.Reservation.Commands.Self.CreateSelfReservation;

public sealed class CreateSelfReservationCommandValidator : ReservationCreateValidatorBase<CreateSelfReservationCommand> 
{
    public CreateSelfReservationCommandValidator()
    {
        RuleFor(x => x.HairdresserId)
            .NotEmpty().WithMessage("ID kadeřníka je povinné.");
    }
}
