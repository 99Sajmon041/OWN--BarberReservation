using FluentValidation;

namespace BarberReservation.Application.Reservation.Common.GetAvailableSlotsForWeek;

public sealed class GetAvailableSlotsForWeekQueryValidator : AbstractValidator<GetAvailableSlotsForWeekQuery>
{
    public GetAvailableSlotsForWeekQueryValidator()
    {
        RuleFor(x => x.HairdresserId)
            .NotEmpty()
            .WithMessage("ID kadeřníka je povinné.")
            .MaximumLength(120)
            .WithMessage("ID kadeřníka může mít maximálně 120 znaků.");

        RuleFor(x => x.ServiceId)
            .GreaterThan(0)
            .WithMessage("ID služby musí být větší než 0.");

        RuleFor(x => x.WeekStartDate)
            .Must(d => d.DayOfWeek == DayOfWeek.Monday)
            .WithMessage("Datum musí být začátek týdne (pondělí).")
            .Must(d => d.Date.AddDays(6) >= DateTime.UtcNow.Date)
            .WithMessage("Týden nesmí být celý v minulosti.");
    }
}
