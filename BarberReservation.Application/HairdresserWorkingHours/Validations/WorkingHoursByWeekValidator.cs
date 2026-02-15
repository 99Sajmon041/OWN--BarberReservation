using BarberReservation.Application.HairdresserWorkingHours.Common;
using FluentValidation;

namespace BarberReservation.Application.HairdresserWorkingHours.Validations;

public class WorkingHoursByWeekValidator<T> : AbstractValidator<T> where T : class, IWorkingHoursWeekFilter
{
    public WorkingHoursByWeekValidator()
    {
        RuleFor(x => x.Monday)
            .Must(x => x.DayOfWeek == DayOfWeek.Monday)
            .WithMessage("Datum musí být pondělí.");
    }
}
