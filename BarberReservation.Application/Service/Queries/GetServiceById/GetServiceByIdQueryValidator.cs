using FluentValidation;

namespace BarberReservation.Application.Service.Queries.GetServiceById;

public sealed class GetServiceByIdQueryValidator : AbstractValidator<GetServiceByIdQuery>
{
    public GetServiceByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID je povinné.");
    }
}
