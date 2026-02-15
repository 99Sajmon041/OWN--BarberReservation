using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using MediatR;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.ExistsWorkingHoursByHairdresser;

public sealed class ExistsWorkingHoursByHairdresserQueryHandler(
    ICurrentAppUser currentAppUser, IUnitOfWork unitOfWork) : IRequestHandler<ExistsWorkingHoursByHairdresserQuery, bool>
{
    public async Task<bool> Handle(ExistsWorkingHoursByHairdresserQuery request, CancellationToken ct)
    {
        var hairdresserId = currentAppUser.User.Id;

        return await unitOfWork.HairdresserWorkingHoursRepository
            .ExistWorkingHoursByHairdresser(hairdresserId, ct);
    }
}
