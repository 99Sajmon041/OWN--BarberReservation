using FluentValidation;

namespace BarberReservation.Application.Reservation.Queries.Hairdresser.GetHairdresserReservationByDay;

public sealed class GetHairdresserReservationByDayQueryValidator : AbstractValidator<GetHairdresserReservationByDayQuery>
{
    public GetHairdresserReservationByDayQueryValidator()
    {
        //RuleFor(x => x.Day)
        //    .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
        //    .WithMessage("Datum nesmí být v minulosti.");
    }
}
