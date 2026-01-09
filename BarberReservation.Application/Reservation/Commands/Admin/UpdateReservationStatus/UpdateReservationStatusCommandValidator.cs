using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Shared.Enums;
using FluentValidation;

namespace BarberReservation.Application.Reservation.Commands.Admin.UpdateReservationStatus;

public sealed class UpdateReservationStatusCommandValidator : IdValidator<UpdateReservationStatusCommand>
{
    public UpdateReservationStatusCommandValidator()
    {
        RuleFor(x => x.NewReservationStatus)
            .Must(x => x is ReservationStatus.Paid or ReservationStatus.NoShow or ReservationStatus.Canceled)
            .WithMessage("Neplatný nový stav rezervace.");

        When(x => x.NewReservationStatus == ReservationStatus.Canceled, () =>
        {
            RuleFor(x => x.CanceledReason)
                .NotNull()
                .WithMessage("Při zrušení rezervace je nutné vyplnit důvod zrušení.");

            RuleFor(x => x.CanceledReason!.Value)
                .IsInEnum()
                .WithMessage("Důvod zrušení je neplatný.");
        });

        When(x => x.NewReservationStatus is not ReservationStatus.Canceled, () =>
        {
            RuleFor(x => x.CanceledReason)
                .Null()
                .WithMessage("Důvod zrušení lze poslat pouze při zrušení rezervace.");
        });
    }
}
