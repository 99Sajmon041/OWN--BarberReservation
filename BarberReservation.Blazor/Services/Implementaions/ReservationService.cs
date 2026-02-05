using BarberReservation.Blazor.Auth;
using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation.Common;

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

        if (!isHairdresser)
        {
            if (!string.IsNullOrWhiteSpace(request.HairdresserId))
                parts.Add($"hairdresserId={Uri.EscapeDataString(request.HairdresserId.Trim())}");
        }

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
}
