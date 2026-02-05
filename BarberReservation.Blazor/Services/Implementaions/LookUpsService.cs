using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Models.LookUpModels;

namespace BarberReservation.Blazor.Services.Implementaions;

public class LookUpsService(IApiClient api) : ILookUpsService
{
    public async Task<List<GetLookUpHairdressers>> GetAllHairdressersAsync(CancellationToken ct)
    {
        return await api.GetAsync<List<GetLookUpHairdressers>>("api/reservations/lookups/hairdressers", ct);
    }

    public async Task<List<ServiceLookUpDto>> GetAllServicesAsync(CancellationToken ct)
    {
        return await api.GetAsync<List<ServiceLookUpDto>>("api/reservations/lookups/services", ct);
    }

    public async Task<List<EnumLookUpDto>> GetAllCanceledByOptionsAsync(CancellationToken ct)
    {
        return await api.GetAsync<List<EnumLookUpDto>>("api/reservations/lookups/reservation-canceled-by", ct);
    }

    public async Task<List<EnumLookUpDto>> GetAllCanceledReasonsAsync(CancellationToken ct)
    {
        return await api.GetAsync<List<EnumLookUpDto>>("api/reservations/lookups/reservation-canceled-reasons", ct);
    }
    public async Task<List<EnumLookUpDto>> GetAllReservationStatusesAsync(CancellationToken ct)
    {
        return await api.GetAsync<List<EnumLookUpDto>>("api/reservations/lookups/reservation-statuses", ct);
    }
}
