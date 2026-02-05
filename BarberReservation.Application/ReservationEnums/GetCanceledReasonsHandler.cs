using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;

namespace BarberReservation.Application.ReservationEnums;

public sealed class GetCanceledReasonsHandler : IRequestHandler<GetEnumLookUpQuery<CanceledReason>, IReadOnlyList<EnumLookUpDto>>
{
    public Task<IReadOnlyList<EnumLookUpDto>> Handle(GetEnumLookUpQuery<CanceledReason> request, CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<EnumLookUpDto>>(
                    Enum.GetValues<CanceledReason>()
                        .Select(v => new EnumLookUpDto { Value = Convert.ToInt32(v), LabelCs = EnumLabelHelper.GetLabelCs(v) })
                        .ToList()
                        .AsReadOnly());
    }
}
