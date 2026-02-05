using BarberReservation.Shared.Models.LookUpModels;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface ILookUpsService
{
    Task<List<GetLookUpHairdressers>> GetAllHairdressersAsync(CancellationToken ct);
    Task<List<ServiceLookUpDto>> GetAllServicesAsync(CancellationToken ct);
    Task<List<EnumLookUpDto>> GetAllCanceledReasonsAsync(CancellationToken ct);
    Task<List<EnumLookUpDto>> GetAllCanceledByOptionsAsync(CancellationToken ct);
    Task<List<EnumLookUpDto>> GetAllReservationStatusesAsync(CancellationToken ct);
}
