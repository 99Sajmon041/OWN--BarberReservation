using BarberReservation.Application.Common.Validation.PagingValidation;

namespace BarberReservation.Application.Common.PagedResultSettings;

public class PagedApiRequest : IHasPaging
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
