using BarberReservation.API.UserIdentity;
using BarberReservation.Application.UserIdentity;

namespace BarberReservation.API.ServiceCollectionApiExtentions;

public static class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {

        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();

        return services;
    }
}
