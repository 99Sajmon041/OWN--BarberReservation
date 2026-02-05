using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;

namespace BarberReservation.Application.ReservationEnums;

public sealed class GetReservationStatusesHandler : IRequestHandler<GetEnumLookUpQuery<ReservationStatus>, IReadOnlyList<EnumLookUpDto>>
{
    public Task<IReadOnlyList<EnumLookUpDto>> Handle(GetEnumLookUpQuery<ReservationStatus> request, CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<EnumLookUpDto>>(
                    Enum.GetValues<ReservationStatus>()
                        .Select(v => new EnumLookUpDto { Value = Convert.ToInt32(v), LabelCs = EnumLabelHelper.GetLabelCs(v) })
                        .ToList()
                        .AsReadOnly());
    }
}
