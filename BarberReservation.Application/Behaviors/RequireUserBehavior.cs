using BarberReservation.Application.Common.Security;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Behaviors;

public sealed class RequireUserBehavior<TRequest, TResponse>(
    ILogger<RequireUserBehavior<TRequest, TResponse>> logger,
    IUserContext userContext,
    ICurrentAppUser currentAppUser,
    UserManager<ApplicationUser> userManager) : IPipelineBehavior<TRequest, TResponse>where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IRequireUser)
            return await next();

        var current = userContext.GetCurrentUser();
        if (current is null)
        {
            logger.LogWarning("User was not recognized (missing/invalid claims). Request: {RequestType}", typeof(TRequest).Name);
            throw new UnauthorizedException("Uživatel nebyl rozpoznán.");
        }

        var appUser = await userManager.FindByIdAsync(current.Id);
        if (appUser is null)
        {
            logger.LogInformation("User with ID: {UserId} was not found. Request: {RequestType}", current.Id, typeof(TRequest).Name);
            throw new UnauthorizedException("Uživatel nebyl nalezen.");
        }

        if (request is IRequireActiveUser && !appUser.IsActive)
        {
            logger.LogWarning("User with ID: {UserId} is not active. Request: {RequestType}", current.Id, typeof(TRequest).Name);
            throw new UnauthorizedException("Uživatel není aktivní.");
        }

        currentAppUser.Initialize(appUser);

        return await next();
    }
}
