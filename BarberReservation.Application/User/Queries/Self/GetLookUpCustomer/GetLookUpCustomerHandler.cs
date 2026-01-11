using BarberReservation.Application.UserIdentity;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Queries.Self.GetLookUpCustomer;

public sealed class GetLookUpCustomerHandler(
    ILogger<GetLookUpCustomerHandler> logger,
    ICurrentAppUser currentAppUser) : IRequestHandler<GetLookUpCustomerQuery, ReservationClientLookUpDto>
{
    public Task<ReservationClientLookUpDto> Handle(GetLookUpCustomerQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = currentAppUser.User;

        var dto = new ReservationClientLookUpDto
        {
            CustomerId = user.Id,
            CustomerName = user.FullName,
            CustomerEmail = user.Email ?? string.Empty,
            CustomerPhone = user.PhoneNumber ?? string.Empty
        };

        logger.LogInformation("Lookup profile returned for user {UserId}.", user.Id);

        return Task.FromResult(dto);
    }
}
