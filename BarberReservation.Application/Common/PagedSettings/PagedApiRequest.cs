using BarberReservation.Application.Common.Validation.PagingValidation;
using BarberReservation.Application.Common.Validation.SearchValidation;

namespace BarberReservation.Application.Common.PagedSettings;

public class PagedApiRequest : IHasPaging, IHasSearch
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Search { get; init; }
    public string? SortBy { get; init; }
    public bool Desc { get; init; } = false;
}
