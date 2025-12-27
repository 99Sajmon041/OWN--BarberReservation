using BarberReservation.Shared.Models.Authorization;
using MediatR;

namespace BarberReservation.Application.Authorization.Command.Login;

public sealed class LoginCommand : IRequest<LoginResponse>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool RememberMe { get; set; }
}
