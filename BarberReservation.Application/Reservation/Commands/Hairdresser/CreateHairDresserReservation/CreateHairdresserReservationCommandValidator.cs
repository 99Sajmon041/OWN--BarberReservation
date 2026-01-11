using BarberReservation.Application.Reservation.Validation;
using FluentValidation;

namespace BarberReservation.Application.Reservation.Commands.Hairdresser.CreateHairDresserReservation;

public sealed class CreateHairdresserReservationCommandValidator : ReservationCreateValidatorBase<CreateHairDresserReservationCommand> 
{
    public CreateHairdresserReservationCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("ID zákazníka nesmí být prázdné.")
            .When(x => x.CustomerId is not null);
    }
}
