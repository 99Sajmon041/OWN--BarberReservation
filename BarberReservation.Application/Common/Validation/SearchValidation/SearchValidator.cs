using FluentValidation;

namespace BarberReservation.Application.Common.Validation.SearchValidation;

public class SearchValidator<T> : AbstractValidator<T> where T : IHasSearch
{
    public SearchValidator()
    {
        RuleFor(x => x.Search)
            .Length(1, 120).WithMessage("Vyhledávání musí mít 1–120 znaků.")
            .When(x => !string.IsNullOrWhiteSpace(x.Search));
    }
}
