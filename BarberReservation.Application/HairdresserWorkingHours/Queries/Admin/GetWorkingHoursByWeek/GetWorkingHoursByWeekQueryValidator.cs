using BarberReservation.Application.HairdresserWorkingHours.Validations;
using FluentValidation;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetWorkingHoursByWeek;

public sealed class GetWorkingHoursByWeekQueryValidator : WorkingHoursByWeekValidator<GetWorkingHoursByWeekQuery>
{
    public GetWorkingHoursByWeekQueryValidator()
    {
        RuleFor(x => x.HairdresserId)
            .NotEmpty().WithMessage("ID kadeřníka nesmí být prázdné.")
            .MaximumLength(120).WithMessage("ID kadeřníka musí být maximálně do 120 znaků.");
    }
}
