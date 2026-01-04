using BarberReservation.Application.Common.Validation.PagingValidation;
using BarberReservation.Application.Common.Validation.SearchValidation;
using FluentValidation;

namespace BarberReservation.Application.HairdresserService.Queries.Self.GetAllSelfHairdressersServices;

public class GetAllSelfHairdressersServicesQueryValidator : AbstractValidator<GetAllSelfHairdressersServicesQuery>
{
    public GetAllSelfHairdressersServicesQueryValidator()
    {
        Include(new PagingValidator<GetAllSelfHairdressersServicesQuery>());
        Include(new SearchValidator<GetAllSelfHairdressersServicesQuery>());

        RuleFor(x => x.ServiceId)
            .GreaterThan(0)
            .When(x => x.ServiceId.HasValue)
            .WithMessage("ServiceId musí být větší než 0.");
    }
}
