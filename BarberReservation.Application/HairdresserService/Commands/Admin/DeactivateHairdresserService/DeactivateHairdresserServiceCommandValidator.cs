using BarberReservation.Application.Common.Validation.IdValidatikon;
using BarberReservation.Application.HairdresserService.Commands.Admin.NewFolder;
using FluentValidation;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.DeactivateHairdresserService;

public sealed class DeactivateHairdresserServiceCommandValidator : IdValidator<DeactivateHairdresserServiceCommand> { }
