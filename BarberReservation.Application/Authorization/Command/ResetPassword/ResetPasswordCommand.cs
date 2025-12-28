using MediatR;

namespace BarberReservation.Application.Authorization.Command.ResetPassword;

public sealed class ResetPasswordCommand : IRequest
{
    public string Email { get; init; } = default!;
    public string Token { get; init; } = default!;
    public string NewPassword { get; init; } = default!;
    public string ConfirmPassword { get; init; } = default!;
}
