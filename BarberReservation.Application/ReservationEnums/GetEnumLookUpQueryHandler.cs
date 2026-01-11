using BarberReservation.Shared.Models.LookUpModels;
using MediatR;

namespace BarberReservation.Application.ReservationEnums;

public sealed class GetEnumLookUpQueryHandler<TEnum> : IRequestHandler<GetEnumLookUpQuery<TEnum>, IReadOnlyList<EnumLookUpDto>> where TEnum : struct, Enum
{
    public Task<IReadOnlyList<EnumLookUpDto>> Handle(GetEnumLookUpQuery<TEnum> request, CancellationToken ct)
    {
        var items = Enum.GetValues<TEnum>()
            .Select(v => new EnumLookUpDto
            {
                Value = Convert.ToInt32(v),
                LabelCs = EnumLabelHelper.GetLabelCs(v)
            })
            .ToList()
            .AsReadOnly();

        return Task.FromResult<IReadOnlyList<EnumLookUpDto>>(items);
    }
}
