using FluentValidation;

namespace BarberReservation.Application.User.Queries.Admin.GetAvailableHairdressersForReservation;

public sealed class GetAvailableHairdressersForReservationQueryValidator : AbstractValidator<GetAvailableHairdressersForReservationQuery>
{
    public GetAvailableHairdressersForReservationQueryValidator()
    {
        RuleFor(x => x.ReservationId)
            .GreaterThan(0)
            .WithMessage("ID rezervace musí být větší než 0.");
    }
}
