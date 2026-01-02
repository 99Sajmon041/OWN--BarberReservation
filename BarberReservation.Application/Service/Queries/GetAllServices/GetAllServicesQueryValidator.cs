using BarberReservation.Application.Common.PagedResultSettings;
using FluentValidation;

namespace BarberReservation.Application.Service.Queries.GetAllServices;

public sealed class GetAllServicesQueryValidator : AbstractValidator<GetAllServicesQuery>
{
    public GetAllServicesQueryValidator()
    {
        RuleFor(x => x.Page)
            .InclusiveBetween(PageSettings.MinimumPage, PageSettings.MaximumPage);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(PageSettings.MinimumPageSize, PageSettings.MaximumPageSize);
    }
}
