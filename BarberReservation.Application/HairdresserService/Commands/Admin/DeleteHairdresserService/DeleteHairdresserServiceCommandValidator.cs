using BarberReservation.Application.Common.Validation.IdValidation;
using FluentValidation;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.DeleteHairdresserService;

public sealed class DeleteHairdresserServiceCommandValidator : IdValidator<DeleteHairdresserServiceCommand> { }
