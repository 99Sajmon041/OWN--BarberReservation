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

        if (failtures.Count > 0)
        {
            var errors = string.Join(" | ", failtures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));

            throw new BarberReservation.Application.Exceptions.ValidationException(errors);
        }

        return await next();
    }
}
