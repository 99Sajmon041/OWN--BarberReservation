using FluentValidation;

namespace BarberReservation.Application.TimeOff.Queries.Hairdresser.GetSelfTimeOffsByDay;

public sealed class GetSelfTimeOffsByDayQueryValidator : AbstractValidator<GetSelfTimeOffsByDayQuery>
{
    public GetSelfTimeOffsByDayQueryValidator()
    {
        RuleFor(x => x.Day)
            .NotEmpty()
            .WithMessage("Den je povinný parametr.")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Datum nesmí být v minulosti.");
    }
}
