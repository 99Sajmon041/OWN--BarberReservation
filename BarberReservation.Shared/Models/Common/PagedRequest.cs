using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Common;

public class PagedRequest
{
    [Range(1, 150, ErrorMessage = "Zobrazená stránka musí být v rozmezí 1 - 150")]
    public int Page { get; set; } = 1;

    [Range(5, 50, ErrorMessage = "Výsledků na stránku musí být v rozmezí 1 - 50")]
    public int PageSize { get; set; } = 20;
    public bool? IsActive { get; set; }
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool Desc { get; set; } = false;
}
