using FluentValidation;

namespace BarberReservation.Application.HairdresserWorkingHours.Commands.Hairdresser.UpsertSelfWorkingHours;

public sealed class UpsertSelfWorkingHoursCommandValidator : AbstractValidator<UpsertSelfWorkingHoursCommand>
{
    private static readonly DayOfWeek[] Weekdays =
    [
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday
    ];

    public UpsertSelfWorkingHoursCommandValidator()
    {
        RuleFor(x => x.DaysOfWorkingWeek)
            .NotNull()
            .NotEmpty()
            .Must(days => days.Count == 5)
            .WithMessage("Musíte poslat přesně 5 dní (Po–Pá).")
            .Must(days => days.Select(d => d.DayOfWeek).Distinct().Count() == 5)
            .WithMessage("Dny musí být unikátní (Po–Pá).")
            .Must(days => days.All(d => Weekdays.Contains(d.DayOfWeek)))
            .WithMessage("Povoleny jsou pouze pracovní dny (Po–Pá).");

        RuleForEach(x => x.DaysOfWorkingWeek)
            .ChildRules(day =>
            {
                day.RuleFor(x => x.DayOfWeek).IsInEnum();

                day.RuleFor(x => x.WorkFrom)
                    .LessThan(x => x.WorkTo)
                    .When(x => x.IsWorkingDay)
                    .WithMessage("Práce-od musí být menší než Práce-do.");
            });
    }
}
