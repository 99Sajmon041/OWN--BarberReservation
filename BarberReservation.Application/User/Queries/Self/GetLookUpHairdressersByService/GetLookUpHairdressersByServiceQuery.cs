using MediatR;

namespace BarberReservation.Application.User.Queries.Self.GetLookUpHairdressersByService;

public sealed class GetLookUpHairdressersByServiceQuery(int serviceId) : IRequest<IReadOnlyList<Shared.Models.LookUpModels.LookUpHairdressersDto>> 
{
    public int ServiceId { get; init; } = serviceId;
}
