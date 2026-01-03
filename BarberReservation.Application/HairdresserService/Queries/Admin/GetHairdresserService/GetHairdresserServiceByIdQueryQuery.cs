using BarberReservation.Shared.Models.HairdresserService;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetHairdresserService;

public sealed class GetHairdresserServiceByIdQueryQuery(int id) : IRequest<HairdresserServiceDto>
{
    public int Id { get; init; } = id;
}
