using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;

namespace BarberReservation.Application.ReservationEnums;

public sealed class GetCanceledByOptionsHandler : IRequestHandler<GetEnumLookUpQuery<ReservationCanceledBy>, IReadOnlyList<EnumLookUpDto>>
{
    public Task<IReadOnlyList<EnumLookUpDto>> Handle(GetEnumLookUpQuery<ReservationCanceledBy> request, CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<EnumLookUpDto>>(
                    Enum.GetValues<ReservationCanceledBy>()
                        .Select(v => new EnumLookUpDto { Value = Convert.ToInt32(v), LabelCs = EnumLabelHelper.GetLabelCs(v) })
                        .ToList()
                        .AsReadOnly());
    }
}
