using BarberReservation.Blazor.Auth;
using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService;

namespace BarberReservation.Blazor.Services.Implementaions;

public sealed class HairdresserService(IApiClient api, AuthState authState) : IHairdresserService
{
    public async Task<PagedResult<HairdresserServiceDto>> GetAllAsync(CommonHairdresserServicePagedRequest request, CancellationToken ct)
    {
        await authState.LoadAsync();

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

        if (request.IsActive is not null)
            parts.Add($"isActive={(request.IsActive.Value ? "true" : "false")}");

        if (authState.Roles.Contains(nameof(UserRoles.Admin)))
        {
            if (!string.IsNullOrWhiteSpace(request.HairdresserId))
                parts.Add($"hairdresserId={Uri.EscapeDataString(request.HairdresserId.Trim())}");
        }

        var qs = string.Join("&", parts);
        string? url;

        if (authState.Roles.Contains(nameof(UserRoles.Admin)))
        {
            url = $"api/admin/hairdresser-services?{qs}";
        }
        else
        {
            url = $"api/me/hairdresser-services?{qs}";
        }

        return await api.GetAsync<PagedResult<HairdresserServiceDto>>(url, ct);
    }

    public async Task<HairdresserServiceDto> GetByIdAsync(int id, CancellationToken ct)
    {
        await authState.LoadAsync();

        if(authState.Roles.Contains(nameof(UserRoles.Admin)))
        {
            return await api.GetAsync<HairdresserServiceDto>($"api/admin/hairdresser-services/{id}", ct);
        }
        else
        {
            return await api.GetAsync<HairdresserServiceDto>($"api/me/hairdresser-services/{id}", ct);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Delete, $"api/admin/hairdresser-services/{id}", null, ct);
    }

    public async Task DeactivateAsync(int id, CancellationToken ct)
    {
        await authState.LoadAsync();

        if(authState.Roles.Contains(nameof(UserRoles.Admin)))
        {
            await api.SendAsync(HttpMethod.Patch, $"api/admin/hairdresser-services/{id}/deactivate", null, ct); 
        }
        else
        {
            await api.SendAsync(HttpMethod.Patch, $"api/me/hairdresser-services/{id}/deactivate", null, ct);
        }
    }

    public async Task ActivateAsync(int id, CancellationToken ct)
    {
        await authState.LoadAsync();

        if (authState.Roles.Contains(nameof(UserRoles.Admin)))
        {
            await api.SendAsync(HttpMethod.Patch, $"api/admin/hairdresser-services/{id}/activate", null, ct);
        }
        else
        {
            await api.SendAsync(HttpMethod.Patch, $"api/me/hairdresser-services/{id}/activate", null, ct);
        }
    }

    public async Task<int> CreateAsync(CreateHairdresserServiceRequest request, CancellationToken ct)
    {
        return await api.PostAsyncWithResponse<CreateHairdresserServiceRequest, int>(HttpMethod.Post, "api/me/hairdresser-services", request, ct);
    }
}
