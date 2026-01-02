using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.Service;

public sealed class UpsertServiceRequest
{
    [Required(ErrorMessage = "Název je povinný.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Název musí být v rozmezí 2 - 100 znaků.")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "Popis je povinný.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Popis musí být v rozmezí 2 - 200 znaků.")]
    public string Description { get; set; } = default!;
}
