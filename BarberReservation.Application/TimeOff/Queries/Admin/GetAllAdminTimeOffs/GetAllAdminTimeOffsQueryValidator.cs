using BarberReservation.Application.TimeOff.Validation;
using FluentValidation;

namespace BarberReservation.Application.TimeOff.Queries.Admin.GetAllAdminTimeOffs;

public sealed class GetAllAdminTimeOffsQueryValidator : TimeOffListQueryValidatorBase<GetAllAdminTimeOffsQuery>
{
    public GetAllAdminTimeOffsQueryValidator()
    {
        RuleFor(x => x.HairdresserId)
            .MaximumLength(120).WithMessage("Maximální povolený počet znaků pro ID je 120.")
            .When(x => !string.IsNullOrWhiteSpace(x.HairdresserId));
    }
}
