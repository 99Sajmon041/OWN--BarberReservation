using FluentValidation;

namespace BarberReservation.Application.Common.Validation.SearchValidation;

public class SearchValidator<T> : AbstractValidator<T> where T : IHasSearch
{
    public SearchValidator()
    {
        RuleFor(x => x.Search)
            .Length(0, 120).WithMessage("Vyhledávání musí mít 0–120 znaků.");
    }
}
