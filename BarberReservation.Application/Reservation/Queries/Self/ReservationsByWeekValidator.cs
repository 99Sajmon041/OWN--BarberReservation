using BarberReservation.Application.Reservation.Common.Interfaces;
using FluentValidation;

namespace BarberReservation.Application.Reservation.Queries.Self;

public class ReservationsByWeekValidator<T> : AbstractValidator<T> where T : class, IReservationsWeekFilter
{
    public ReservationsByWeekValidator()
    {
        RuleFor(x => x.Monday)
            .Must(x => x.DayOfWeek == DayOfWeek.Monday)
            .WithMessage("Datum musí být pondělí.");
    }
}
