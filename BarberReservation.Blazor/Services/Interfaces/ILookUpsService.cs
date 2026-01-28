using BarberReservation.Shared.Models.LookUpModels;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface ILookUpsService
{
    Task<List<GetLookUpHairdressers>> GetAllHairdressersAsync(CancellationToken ct);
    Task<List<ServiceLookUpDto>> GetAllServicesAsync(CancellationToken ct);
}
