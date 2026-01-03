using FluentValidation;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.DeleteHairdresserService;

public sealed class DeleteHairdresserServiceCommandValidator : AbstractValidator<DeleteHairdresserServiceCommand>
{
	public DeleteHairdresserServiceCommandValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0).WithMessage("ID je povinný.");
	}
}
