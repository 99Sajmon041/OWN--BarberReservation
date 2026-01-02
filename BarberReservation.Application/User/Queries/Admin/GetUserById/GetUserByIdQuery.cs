using BarberReservation.Shared.Models.User.Common;
using MediatR;

namespace BarberReservation.Application.User.Queries.Admin.GetUserById;

public sealed class GetUserByIdQuery(string id) : IRequest<UserDto>
{
    public string Id { get; init; } = id;
}
