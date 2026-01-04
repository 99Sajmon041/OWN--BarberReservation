using FluentValidation;

namespace BarberReservation.Application.Service.Validation;

public static class ServiceUpsertRules
{
    public static void Apply<T>(IRuleBuilderInitial<T, string> name, IRuleBuilderInitial<T, string> description)
    {
        name
            .NotEmpty().WithMessage("Název je povinný.")
            .Length(2, 100).WithMessage("Název musí být v rozmezí 2 - 100 znaků.");

        description
            .NotEmpty().WithMessage("Popis je povinný.")
            .Length(2, 200).WithMessage("Popis musí být v rozmezí 2 - 200 znaků.");
    }
}
