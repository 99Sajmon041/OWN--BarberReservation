namespace BarberReservation.Shared.Models.Common;

public sealed class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItemsCount { get; set; }
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalItemsCount / PageSize);
    public bool CanGoNext => Page < TotalPages;
    public bool CanGoPrevious => Page > 1;
}
