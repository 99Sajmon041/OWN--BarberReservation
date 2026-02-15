using BarberReservation.Application.TimeOff.Validation;
using FluentValidation;

namespace BarberReservation.Application.TimeOff.Queries.Admin.GetAllAdminTimeOffsWeekly;

public sealed class GetAllAdminTimeOffsWeeklyQueryValidator : TimeOffDailyListQueryValidatorBase<GetAllAdminTimeOffsWeeklyQuery> 
{
    public GetAllAdminTimeOffsWeeklyQueryValidator()
    {
        RuleFor(x => x.HairdresserId)
            .NotEmpty().WithMessage("ID kadeřníka nesmí být prázdné.")
            .MaximumLength(120).WithMessage("ID kadeřníka musí být maximálně do 120 znaků.");
    }
}