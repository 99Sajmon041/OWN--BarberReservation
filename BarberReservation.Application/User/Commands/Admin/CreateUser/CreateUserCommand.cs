using MediatR;

namespace BarberReservation.Application.User.Commands.Admin.CreateUser;

public sealed class CreateUserCommand : IRequest<string>
{
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string Role { get; init; } = default!;
}
