using FluentValidation;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetNextWorkingHoursForHairdresser;

public sealed class GetNextWorkingHoursForHairdresserQueryValidator : AbstractValidator<GetNextWorkingHoursForHairdresserQuery>
{
    public GetNextWorkingHoursForHairdresserQueryValidator()
    {
        RuleFor(x => x.HairdresserId)
            .NotEmpty().WithMessage("ID kadeřníka je povinný údaj.");
    }
}
