using BarberReservation.Shared.Models.Authorization;
using MediatR;

namespace BarberReservation.Application.Authorization.Command.Login;

public sealed class LoginCommand : IRequest<LoginResponse>
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public bool RememberMe { get; init; }
}
