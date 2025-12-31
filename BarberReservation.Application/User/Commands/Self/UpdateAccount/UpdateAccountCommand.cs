using BarberReservation.Application.Common.Security;
using MediatR;
namespace BarberReservation.Application.User.Commands.Self.UpdateAccount;

public sealed class UpdateAccountCommand : IRequest, IRequireActiveUser
{
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
}
