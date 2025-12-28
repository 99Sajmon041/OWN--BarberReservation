using BarberReservation.Application.Authorization.Command.ChangePassword;
using BarberReservation.Application.Authorization.Command.ForgotPassword;
using BarberReservation.Application.Authorization.Command.Login;
using BarberReservation.Application.Authorization.Command.Register;
using BarberReservation.Application.Authorization.Command.ResetPassword;
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

    public static ChangePasswordCommand GetChangePasswordCommand(this ChangePasswordRequest request)
    {
        return new ChangePasswordCommand
        {
            OldPassword = request.OldPassword,
            NewPassword = request.NewPassword,
            ConfirmPassword = request.ConfirmPassword,
        };
    }

    public static RegisterCommand GetRegisterCommand(this RegisterRequest request)
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

    public static ForgotPasswordCommand GetForgotPasswordCommand(this ForgotPasswordRequest request)
    {
        return new ForgotPasswordCommand
        {
            Email = request.Email
        };
    }

    public static ResetPasswordCommand GetResetPasswordCommand(this ResetPasswordRequest request)
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
