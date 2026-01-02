using FluentValidation;

namespace BarberReservation.Application.User.Queries.Admin.GetUserById;

public sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID je povinný.");
    }
}
