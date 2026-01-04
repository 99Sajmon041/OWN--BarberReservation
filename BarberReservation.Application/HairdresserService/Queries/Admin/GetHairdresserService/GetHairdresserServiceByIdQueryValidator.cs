using BarberReservation.Application.Common.Validation.IdValidatikon;
using FluentValidation;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetHairdresserService;

public sealed class GetHairdresserServiceByIdQueryValidator : IdValidator<GetHairdresserServiceByIdQueryQuery> { }
