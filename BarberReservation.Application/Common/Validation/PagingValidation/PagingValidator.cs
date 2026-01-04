using BarberReservation.Application.Common.PagedSettings;
using FluentValidation;

namespace BarberReservation.Application.Common.Validation.PagingValidation;

public class PagingValidator<T> : AbstractValidator<T> where T : IHasPaging
{
    public PagingValidator()
    {
        RuleFor(x => x.Page)
            .InclusiveBetween(PageSettings.MinimumPage, PageSettings.MaximumPage)
            .WithMessage("Stránka musí být v rozmezí 1 - 150.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(PageSettings.MinimumPageSize, PageSettings.MaximumPageSize)
            .WithMessage("Počet výsledků na stránku musí být v rozmezí 5 - 50.");
    }
}
