using FluentValidation;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetSelfDailyWorkingHours;

public sealed class GetSelfDailyWorkingHoursQueryValidator : AbstractValidator<GetSelfDailyWorkingHoursQuery>
{
    public GetSelfDailyWorkingHoursQueryValidator()
    {
        RuleFor(x => x.Day)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Datum nesmí být v minulosti.");
    }
}
