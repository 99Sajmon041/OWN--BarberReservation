using BarberReservation.Application.Authorization.Command.Login;
using BarberReservation.Application.Behaviors;
using BarberReservation.Application.User.Mapping;
using BarberReservation.Application.UserIdentity;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BarberReservation.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICurrentAppUser, CurrentAppUser>();

        services.AddAutoMapper(cfg => { }, typeof(UserMappingProfile).Assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequireUserBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequirePasswordChangeBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(LoginCommandValidator).Assembly);

        services.AddMediatR(typeof(LoginCommandHandler).Assembly);

        return services;
    }
}
