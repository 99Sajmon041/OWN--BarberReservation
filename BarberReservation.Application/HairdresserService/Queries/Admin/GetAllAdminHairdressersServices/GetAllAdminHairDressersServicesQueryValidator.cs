using BarberReservation.Application.Common.Validation.PagingValidation;
using BarberReservation.Application.Common.Validation.SearchValidation;
using FluentValidation;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetAllAdminHairdressersServices;

public sealed class GetAllAdminHairdressersServicesQueryValidator : AbstractValidator<GetAllAdminHairdressersServicesQuery>
{
    public GetAllAdminHairdressersServicesQueryValidator()
    {
        Include(new PagingValidator<GetAllAdminHairdressersServicesQuery>());
        Include(new SearchValidator<GetAllAdminHairdressersServicesQuery>());

        RuleFor(x => x.ServiceId)
            .GreaterThan(0)
            .When(x => x.ServiceId.HasValue)
            .WithMessage("ID služby musí být větší než 0.");

        RuleFor(x => x.HairdresserId)
            .MaximumLength(120)
            .When(x => !string.IsNullOrWhiteSpace(x.HairdresserId))
            .WithMessage("ID kadeřníka musí mít maximálně 120 znaků.");
    }
}
