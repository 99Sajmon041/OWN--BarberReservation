using FluentValidation;

namespace BarberReservation.Application.Reservation.Commands.Admin.ChangeReservationHairdresser;

public sealed class ChangeReservationHairdresserCommandValidator : AbstractValidator<ChangeReservationHairdresserCommand>
{
    public ChangeReservationHairdresserCommandValidator()
    {
        RuleFor(x => x.ReservationId)
            .GreaterThan(0)
            .WithMessage("ID rezervace musí být větší než 0.");

        RuleFor(x => x.HairdresserId)
            .NotEmpty().WithMessage("ID kadeřníka je povinné.")
            .MaximumLength(120).WithMessage("ID kadeřníka musí mít maximálně 120 znaků.");
    }
}
