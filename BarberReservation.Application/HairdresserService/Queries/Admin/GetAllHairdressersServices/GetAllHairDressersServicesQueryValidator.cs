using BarberReservation.Application.Common.PagedResultSettings;
using FluentValidation;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetAllHairdresserServices;

public sealed class GetAllHairDressersServicesQueryValidator : AbstractValidator<GetAllHairdresserServicesQuery>
{
    public GetAllHairDressersServicesQueryValidator()
    {
        RuleFor(x => x.Page)
            .InclusiveBetween(PageSettings.MinimumPage, PageSettings.MaximumPage);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(PageSettings.MinimumPageSize, PageSettings.MaximumPageSize);
    }
}
