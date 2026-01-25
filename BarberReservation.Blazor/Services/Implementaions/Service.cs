using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Service;

namespace BarberReservation.Blazor.Services.Implementaions;

public sealed class Service(IApiClient api) : IService
{
    public async Task<PagedResult<ServiceDto>> GetAllAsync(ServicePageRequest request, CancellationToken ct)
    {
        var parts = new List<string>
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

        if (request.IsActive is not null)
            parts.Add($"isActive={(request.IsActive.Value ? "true" : "false")}");

        var qs = string.Join("&", parts);
        var url = string.IsNullOrEmpty(qs) ? "api/services" : $"api/services?{qs}";

        return await api.GetAsync<PagedResult<ServiceDto>>(url, ct);
    }

    public async Task<ServiceDto> GetByIdAsync(int id, CancellationToken ct)
    {
        return await api.GetAsync<ServiceDto>($"api/services/{id}", ct);
    }

    public async Task ActivateByIdAsync(int id, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Patch, $"api/services/{id}/activate", null, ct);
    }

    public async Task DeactivateByIdAsync(int id, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Patch, $"api/services/{id}/deactivate", null, ct);
    }
}
