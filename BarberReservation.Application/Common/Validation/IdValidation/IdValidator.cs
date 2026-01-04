using FluentValidation;

namespace BarberReservation.Application.Common.Validation.IdValidation;

public class IdValidator<T> : AbstractValidator<T> where T : IHasId
{
    public IdValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID musí být číslo a větší než 0.");
    }
}
