using MediatR;

namespace BarberReservation.Application.Authorization.Command.ChangePassword;

public sealed class ChangePasswordCommand : IRequest
{
    public string OldPassword { get; init; } = default!;
    public string NewPassword { get; init; } = default!;
    public string ConfirmPassword { get; init; } = default!;
}
