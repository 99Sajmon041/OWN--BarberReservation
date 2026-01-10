using BarberReservation.Application.UserIdentity;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Queries.Self.GetLookUpProfile;

public sealed class GetLookUpProfileHandler(
    ILogger<GetLookUpProfileHandler> logger,
    ICurrentAppUser currentAppUser) : IRequestHandler<GetLookUpProfileQuery, ReservationClientLookupDto>
{
    public Task<ReservationClientLookupDto> Handle(GetLookUpProfileQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = currentAppUser.User;

        var dto = new ReservationClientLookupDto
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
