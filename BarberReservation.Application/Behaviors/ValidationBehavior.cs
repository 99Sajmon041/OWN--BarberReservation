using FluentValidation;
using MediatR;

namespace BarberReservation.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var failtures = validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if(failtures.Count > 0)
        {
            var errors = failtures
                .GroupBy(f => f.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).Distinct().ToArray());

            throw new BarberReservation.Application.Exceptions.ValidationException("Neplatný vstup", errors);
        }

        return await next();
    }
}
