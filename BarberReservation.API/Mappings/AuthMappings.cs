using BarberReservation.Application.Authorization.Command.ChangePassword;
using BarberReservation.Application.Authorization.Command.ForgotPassword;
using BarberReservation.Application.Authorization.Command.Login;
using BarberReservation.Application.Authorization.Command.Register;
using BarberReservation.Application.Authorization.Command.ResetPassword;
using BarberReservation.Shared.Models.Authorization;

namespace BarberReservation.API.Mappings;

public static class AuthMappings
{
    public static LoginCommand ToLoginCommand(this LoginRequest request)
    {
        return new LoginCommand
        {
            Email = request.Email,
            Password = request.Password,
            RememberMe = request.RememberMe
        };
    }

    public static ChangePasswordCommand ToChangePasswordCommand(this ChangePasswordRequest request)
    {
        return new ChangePasswordCommand
        {
            OldPassword = request.OldPassword,
            NewPassword = request.NewPassword,
            ConfirmPassword = request.ConfirmPassword,
        };
    }

    public static RegisterCommand ToRegisterCommand(this RegisterRequest request)
    {
        return new RegisterCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            PhoneNumber = request.PhoneNumber
        };
    }

    public static ForgotPasswordCommand ToForgotPasswordCommand(this ForgotPasswordRequest request)
    {
        return new ForgotPasswordCommand
        {
            Email = request.Email
        };
    }

    public static ResetPasswordCommand ToResetPasswordCommand(this ResetPasswordRequest request)
    {
        return new ResetPasswordCommand
        {
            Email = request.Email,
            Token = request.Token,
            NewPassword = request.NewPassword,
            ConfirmPassword = request.ConfirmPassword
        };   
    }
}
