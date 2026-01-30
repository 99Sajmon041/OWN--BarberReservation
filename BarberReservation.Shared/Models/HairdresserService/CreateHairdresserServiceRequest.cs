using System.ComponentModel.DataAnnotations;

namespace BarberReservation.Shared.Models.HairdresserService;

public sealed class CreateHairdresserServiceRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "ID služby musí být větší než 0.")]
    public int ServiceId { get; set; }
    [Range(10, 100, ErrorMessage = "Služba musí trvat v rozmezí 10 - 100 minut.")]
    public int DurationMinutes { get; set; }

    [Range(100, 5000, ErrorMessage = "Cena musí být v rozmezí 100 - 5 000,- Kč.")]
    public decimal Price { get; set; }
}
