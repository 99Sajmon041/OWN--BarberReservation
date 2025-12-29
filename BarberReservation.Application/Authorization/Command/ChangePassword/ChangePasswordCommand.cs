using BarberReservation.Application.Common.Security;
using MediatR;

namespace BarberReservation.Application.Authorization.Command.ChangePassword;

public sealed class ChangePasswordCommand : IRequest, IRequireActiveUser
{
    public string OldPassword { get; init; } = default!;
    public string NewPassword { get; init; } = default!;
    public string ConfirmPassword { get; init; } = default!;
}
