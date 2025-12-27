using BarberReservation.Application.UserIdentity;
using Microsoft.Extensions.DependencyInjection;

namespace BarberReservation.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();

        return services;
    }
}
