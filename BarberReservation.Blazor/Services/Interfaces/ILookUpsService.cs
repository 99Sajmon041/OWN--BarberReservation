using BarberReservation.Shared.Models.LookUpModels;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface ILookUpsService
{
    Task<List<LookUpHairdressersDto>> GetAllHairdressersAsync(CancellationToken ct);
    Task<List<LookUpHairdressersDto>> GetAllHairdressersByServiceAsync(int serviceId, CancellationToken ct);
    Task<List<ServiceLookUpDto>> GetAllServicesAsync(CancellationToken ct);
    Task<List<EnumLookUpDto>> GetAllCanceledReasonsAsync(CancellationToken ct);
    Task<List<EnumLookUpDto>> GetAllCanceledByOptionsAsync(CancellationToken ct);
    Task<List<EnumLookUpDto>> GetAllReservationStatusesAsync(CancellationToken ct);
    Task<List<ReservationClientLookUpDto>> GetAllCustomersAsync(string search, CancellationToken ct);
    Task<List<LookUpHairdressersDto>> GetAllActiveHairdressersWithWorkingHoursAsync(CancellationToken ct);
}
