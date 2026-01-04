using BarberReservation.Application.Common.Validation.IdValidatikon;
using FluentValidation;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.DeleteHairdresserService;

public sealed class DeleteHairdresserServiceCommandValidator : IdValidator<DeleteHairdresserServiceCommand> { }
