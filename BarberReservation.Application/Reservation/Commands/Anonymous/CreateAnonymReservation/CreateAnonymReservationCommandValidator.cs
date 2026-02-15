using BarberReservation.Application.Reservation.Validation;
using FluentValidation;

namespace BarberReservation.Application.Reservation.Commands.Anonymous.CreateAnonymReservation;

public sealed class CreateAnonymReservationCommandValidator : ReservationCreateValidatorBase<CreateAnonymReservationCommand>
{
    public CreateAnonymReservationCommandValidator()
    {
        RuleFor(x => x.HairdresserId)
            .NotEmpty().WithMessage("ID kadeřníka je povinné.");
    }
}
