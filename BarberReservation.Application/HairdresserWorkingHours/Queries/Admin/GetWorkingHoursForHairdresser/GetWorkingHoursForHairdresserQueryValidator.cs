using FluentValidation;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetWorkingHoursForHairdresser;

public sealed class GetWorkingHoursForHairdresserQueryValidator : AbstractValidator<GetWorkingHoursForHairdresserQuery>
{
    public GetWorkingHoursForHairdresserQueryValidator()
    {
        RuleFor(x => x.HairdresserId)
            .NotEmpty().WithMessage("ID kadeřníka je povinný údaj.");
    }
}
