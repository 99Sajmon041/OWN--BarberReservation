using BarberReservation.Shared.Models.LookUpModels;
using MediatR;

namespace BarberReservation.Application.User.Queries.Admin.GetAvailableHairdressersForReservation;

public sealed class GetAvailableHairdressersForReservationQuery(int reservationId) : IRequest<List<GetLookUpHairdressers>>
{
    public int ReservationId { get; init; } = reservationId;
}
