using MediatR;

namespace BarberReservation.Application.Authorization.Command.ForgotPassword;

public sealed class ForgotPasswordCommand : IRequest
{
    public string Email { get; init; } = default!;
}
