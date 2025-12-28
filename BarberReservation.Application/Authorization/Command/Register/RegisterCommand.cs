using MediatR;

namespace BarberReservation.Application.Authorization.Command.Register;

public sealed class RegisterCommand : IRequest
{
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
}
