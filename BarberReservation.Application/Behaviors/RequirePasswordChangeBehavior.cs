using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using MediatR;

namespace BarberReservation.Application.Behaviors;

public sealed class RequirePasswordChangeBehavior<TRequest, TResponse>(ICurrentAppUser currentAppUser) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken ct, RequestHandlerDelegate<TResponse> next)
    {
        if (request is not IRequireUser)
            return await next();

        if (request is IAllowMustChangePassword)
            return await next();

        var user = currentAppUser.User;

        if(user.MustChangePassword)
            throw new ForbiddenException("Nejprve si musíte změnit heslo.");

        return await next();
    }
}
