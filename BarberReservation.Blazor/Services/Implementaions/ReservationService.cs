using BarberReservation.Blazor.Auth;
using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation;

namespace BarberReservation.Blazor.Services.Implementaions;

public sealed class ReservationService(IApiClient api, AuthState authState) : IReservationService
{
    public async Task<List<ReservationDto>> GetByDayAsync(DateOnly day, CancellationToken ct)
    {
        return await api.GetAsync<List<ReservationDto>>($"api/hairdresser/reservations/daily?day={day:yyyy-MM-dd}", ct);
    }
    public async Task<PagedResult<ReservationDto>> GetAllAsync(ReservationPagedRequest request, CancellationToken ct)
    {
        await authState.LoadAsync();

        bool isHairdresser = authState.Roles.Contains(nameof(UserRoles.Hairdresser));
        bool isAdmin = authState.Roles.Contains(nameof(UserRoles.Admin));
        bool isCustomer = authState.Roles.Contains(nameof(UserRoles.Customer));

        var parts = new List<string>()
        {
            $"page={request.Page}",
            $"pageSize={request.PageSize}"
        };

        if (!string.IsNullOrWhiteSpace(request.Search))
            parts.Add($"search={Uri.EscapeDataString(request.Search.Trim())}");

        if (!string.IsNullOrWhiteSpace(request.SortBy))
            parts.Add($"sortBy={Uri.EscapeDataString(request.SortBy.Trim())}");

        if (request.Desc)
            parts.Add("desc=true");

        if (request.ServiceId is not null)
            parts.Add($"serviceId={request.ServiceId}");

        if ((isAdmin || isCustomer) && !string.IsNullOrWhiteSpace(request.HairdresserId))
            parts.Add($"hairdresserId={Uri.EscapeDataString(request.HairdresserId.Trim())}");

        if (isAdmin && !string.IsNullOrWhiteSpace(request.CustomerId))
            parts.Add($"customerId={Uri.EscapeDataString(request.CustomerId.Trim())}");

        if (request.Status is not null)
            parts.Add($"status={request.Status}");

        if (request.CanceledBy is not null)
            parts.Add($"canceledBy={request.CanceledBy}");

        if (request.CanceledReason is not null)
            parts.Add($"canceledReason={request.CanceledReason}");

        if (request.CreatedFrom is not null)
            parts.Add($"createdFrom={Uri.EscapeDataString(request.CreatedFrom.Value.ToString("O"))}");

        if (request.CreatedTo is not null)
            parts.Add($"createdTo={Uri.EscapeDataString(request.CreatedTo.Value.ToString("O"))}");

        if (request.StartFrom is not null)
            parts.Add($"startFrom={Uri.EscapeDataString(request.StartFrom.Value.ToString("O"))}");

        if (request.StartTo is not null)
            parts.Add($"startTo={Uri.EscapeDataString(request.StartTo.Value.ToString("O"))}");

        if (request.CanceledFrom is not null)
            parts.Add($"canceledFrom={Uri.EscapeDataString(request.CanceledFrom.Value.ToString("O"))}");

        if (request.CanceledTo is not null)
            parts.Add($"canceledTo={Uri.EscapeDataString(request.CanceledTo.Value.ToString("O"))}");

        var qs = string.Join("&", parts);
        string url = string.Empty;

        if (isAdmin)
        {
            url = $"api/admin/reservations?{qs}";
        }
        else if (isHairdresser)
        {
            url = $"api/hairdresser/reservations?{qs}";
        }
        else
        {
            url = $"api/me/reservations?{qs}";
        }

        return await api.GetAsync<PagedResult<ReservationDto>>(url, ct);
    }

    public async Task UpdateStatusAsync(int id, UpdateReservationStatusRequest request, CancellationToken ct)
    {
        await authState.LoadAsync();

        if (authState.Roles.Contains(nameof(UserRoles.Admin)))
            await api.SendAsync(HttpMethod.Patch, $"api/admin/reservations/{id}/status", request, ct);
        else if (authState.Roles.Contains(nameof(UserRoles.Hairdresser)))
            await api.SendAsync(HttpMethod.Patch, $"api/hairdresser/reservations/{id}/status", request, ct);
        else
            throw new ApiRequestException("Uživatel nemá právo měnit status rezervace.", StatusCodes.Status403Forbidden);
    }

    public async Task CancelReservationByClientAsync(int id, CancellationToken ct)
    {
        await authState.LoadAsync();

        if (authState.Roles.Contains(nameof(UserRoles.Customer)))
            await api.SendAsync(HttpMethod.Patch, $"api/me/reservations/{id}/cancel", null, ct);
        else
            throw new ApiRequestException("Nemáte oprávnění zrušit rezervaci.", StatusCodes.Status403Forbidden);
    }

    public async Task<ReservationDto> GetByIdAsync(int id, CancellationToken ct)
    {
        await authState.LoadAsync();

        string url = authState.Roles switch
        {
            var r when r.Contains(nameof(UserRoles.Admin)) => $"api/admin/reservations/{id}",
            var r when r.Contains(nameof(UserRoles.Hairdresser)) => $"api/hairdresser/reservations/{id}",
            var r when r.Contains(nameof(UserRoles.Customer)) => $"api/me/reservations/{id}",
            _ => throw new ApiRequestException("Nemáte oprávnění zobrazit rezervaci.", StatusCodes.Status403Forbidden)
        };

        return await api.GetAsync<ReservationDto>(url, ct);
    }

    public async Task ChangeReservationHairdresserAsync(int reservationId, string hairdresserId, CancellationToken ct)
    {
        var encoded = Uri.EscapeDataString(hairdresserId.Trim());
        await api.SendAsync(HttpMethod.Put, $"api/admin/reservations/{reservationId}/hairdresser?hairdresserId={encoded}", null, ct);
    }

    public async Task<int> CreateReservationAsync(CreateReservationRequest request, CancellationToken ct)
    {
        await authState.LoadAsync();
        if (authState.Roles.Contains(nameof(UserRoles.Admin)))
        {
            return await api.PostAsyncWithResponse<CreateReservationRequest, int>(HttpMethod.Post, "api/admin/reservations", request, ct);
        }
        else if (authState.Roles.Contains(nameof(UserRoles.Hairdresser)))
        {
            return await api.PostAsyncWithResponse<CreateReservationRequest, int>(HttpMethod.Post, "api/hairdresser/reservations", request, ct);
        }
        else
        {
            throw new ApiRequestException("Nemáte dostatečné práva pro tuto akci.", StatusCodes.Status403Forbidden);
        }
    }

    public async Task CreateReservationAsCustomerAsync(CreateReservationRequest request, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Post, "api/me/reservations", request, ct);
    }

    public async Task<List<SlotDto>> GetFreeSlotsForWeekAsync(string hairdresserId, DateTime weekStartDate, int serviceId, CancellationToken ct)
    {
        var url = $"hairdresserId={Uri.EscapeDataString(hairdresserId.Trim())}" +
            $"&serviceId={Uri.EscapeDataString(serviceId.ToString())}" +
            $"&weekStartDate={Uri.EscapeDataString(weekStartDate.ToString("yyyy-MM-dd"))}";

        return await api.GetAsync<List<SlotDto>>($"api/me/reservations/available-slots?{url}", ct);
    }
}
