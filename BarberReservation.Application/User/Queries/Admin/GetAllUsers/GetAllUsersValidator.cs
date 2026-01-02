using BarberReservation.Application.Common.PagedResultSettings;
using FluentValidation;

namespace BarberReservation.Application.User.Queries.Admin.GetAllUsers;

public sealed class GetAllUsersValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersValidator()
    {
        RuleFor(x => x.Page)
            .InclusiveBetween(PageSettings.MinimumPage, PageSettings.MaximumPage);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(PageSettings.MinimumPageSize, PageSettings.MaximumPageSize);
    }
}
