using BarberReservation.Application.Common.Security;
using MediatR;

namespace BarberReservation.Application.User.Commands.Admin.UpdateEmail;

public sealed class UpdateEmailCommand : IRequireActiveUser, IRequest
{
    public string Id { get; init; } = default!;
    public string Email { get; init; } = default!;
}
