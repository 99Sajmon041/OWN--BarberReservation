using BarberReservation.Application.Authorization.Command.Login;
using BarberReservation.Shared.Models.Authorization;

namespace BarberReservation.API.Mappings;

public static class AuthMappings
{
    public static LoginCommand GetLoginCommand(this LoginRequest request)
    {
        return new LoginCommand
        {
            Email = request.Email,
            Password = request.Password,
            RememberMe = request.RememberMe
        };
    }
}
