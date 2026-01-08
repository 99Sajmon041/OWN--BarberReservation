using BarberReservation.Application.Common.Validation.IdValidation;
using FluentValidation;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetHairdresserService;

public sealed class GetHairdresserServiceByIdQueryValidator : IdValidator<GetHairdresserServiceByIdQuery> { }
