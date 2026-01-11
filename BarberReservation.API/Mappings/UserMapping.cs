using BarberReservation.Application.User.Commands.Admin.CreateHairdresser;
using BarberReservation.Application.User.Commands.Admin.PartlyUpdateUser;
using BarberReservation.Application.User.Commands.Admin.UpdateEmail;
using BarberReservation.Application.User.Commands.Self.UpdateAccount;
using BarberReservation.Shared.Models.User.Admin;
using BarberReservation.Shared.Models.User.Common;

namespace BarberReservation.API.Mappings;

public static class UserMapping
{
    public static UpdateAccountCommand ToUpdateAccountCommand(this UpdateUserRequest request)
    {
        return new UpdateAccountCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber
        };
    }

    public static CreateHairdresserCommand ToCreateAccountCommand(this CreateUserRequest request)
    {
        return new CreateHairdresserCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };
    }

    public static PartlyUpdateUserCommand ToUpdateUserCommand(this UpdateUserRequest request, string id)
    {
        return new PartlyUpdateUserCommand
        {
            Id = id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber
        };
    }

    public static UpdateEmailCommand ToUpdateUserEmailCommnad(this UpdateUserEmailRequest request, string id)
    {
        return new UpdateEmailCommand
        {
            Id = id,
            Email = request.Email
        };
    }
}
