using FluentValidation;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetHairdresserService;

public sealed class GetHairdresserServiceByIdQueryValidator : AbstractValidator<GetHairdresserServiceByIdQueryQuery>
{
    public GetHairdresserServiceByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID je povinné.");
    }
}
