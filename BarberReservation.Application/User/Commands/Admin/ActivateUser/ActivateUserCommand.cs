using MediatR;

namespace BarberReservation.Application.User.Commands.Admin.ActivateUser;

public sealed class ActivateUserCommand(string id) : IRequest
{
    public string Id { get; set; } = id;
}