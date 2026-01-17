using BarberReservation.Application.Common.Validation.IdValidation;
using BarberReservation.Application.TimeOff.Validation;

namespace BarberReservation.Application.TimeOff.Commands.Hairdresser.UpdateSelfTimeOff;

public sealed class UpdateSelfTimeOffCommandValidator : TimeOffUpsertValidatorBase<UpdateSelfTimeOffCommand> 
{
    public UpdateSelfTimeOffCommandValidator()
    {
        Include(new IdValidator<UpdateSelfTimeOffCommand>());
    }
}