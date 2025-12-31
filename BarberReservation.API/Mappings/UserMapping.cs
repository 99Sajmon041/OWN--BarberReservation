using BarberReservation.Application.User.Commands.Self.UpdateAccount;
using BarberReservation.Shared.Models.User.Self;

namespace BarberReservation.API.Mappings;

public static class UserMapping
{
    public static UpdateAccountCommand GetUpdateAccountCommand(this UpdateUserRequest request)
    {
        return new UpdateAccountCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber
        };
    }
}
