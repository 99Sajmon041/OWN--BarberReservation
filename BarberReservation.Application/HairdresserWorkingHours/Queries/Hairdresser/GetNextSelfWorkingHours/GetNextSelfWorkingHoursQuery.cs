using BarberReservation.Application.Common.Security;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;
using MediatR;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetNextSelfWorkingHours;

public sealed class GetNextSelfWorkingHoursQuery : IRequireActiveUser, IRequest<HairdresserWorkingHoursDto> { }
