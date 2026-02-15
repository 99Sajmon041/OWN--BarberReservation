using BarberReservation.Application.TimeOff.Common;
using FluentValidation;

namespace BarberReservation.Application.TimeOff.Validation;

public class TimeOffDailyListQueryValidatorBase<T> : AbstractValidator<T> where T : class, ITimeOffDailyListFilter
{
    public TimeOffDailyListQueryValidatorBase()
    {
        RuleFor(x => x.WeekStartDate)
            .Must(x => x.DayOfWeek == DayOfWeek.Monday)
            .WithMessage("Datum musí být pondělí.");
    }
}
