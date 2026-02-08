using BarberReservation.Blazor.Auth;
using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.LookUpModels;

namespace BarberReservation.Blazor.Services.Implementaions;

public class LookUpsService(IApiClient api, AuthState authState) : ILookUpsService
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

    public async Task<List<ReservationClientLookUpDto>> GetAllCustomersAsync(string search, CancellationToken ct)
    {
        search = (search ?? "").Trim();
        if (search.Length < 3) return [];

        await authState.LoadAsync();

        if (!authState.IsAuthenticated || !(authState.Roles.Contains(nameof(UserRoles.Admin)) || authState.Roles.Contains(nameof(UserRoles.Hairdresser))))
            throw new ApiRequestException("Nemáte právo na načtení zákazníků.", StatusCodes.Status403Forbidden);

        var q = Uri.EscapeDataString(search);
        return await api.GetAsync<List<ReservationClientLookUpDto>>($"api/reservations/lookups/customers?search={q}", ct);
    }

    public async Task<List<GetLookUpHairdressers>> GetAllHairdressersByServiceAsync(int serviceId, CancellationToken ct)
    {
        return await api.GetAsync<List<GetLookUpHairdressers>>($"api/reservations/lookups/service/{serviceId}/hairdressers", ct);
    }
}
