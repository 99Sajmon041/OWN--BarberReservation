using BarberReservation.Shared.Models.LookUpModels;
using MediatR;

namespace BarberReservation.Application.ReservationEnums;

public sealed class GetEnumLookUpQuery<TEnum> : IRequest<IReadOnlyList<EnumLookUpDto>> where TEnum : struct, Enum { }
