using BarberReservation.Application.Common.Security;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;
using MediatR;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetSelfWorkingHours;

public sealed class GetSelfWorkingHoursQuery : IRequireActiveUser, IRequest<IReadOnlyList<HairdresserWorkingHoursDto>> { }
