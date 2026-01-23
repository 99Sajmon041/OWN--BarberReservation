using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.User.Admin;
using BarberReservation.Shared.Models.User.Common;

namespace BarberReservation.Blazor.Services.Implementaions;

public class UserService(IApiClient api) : IUserService
{
    public async Task<PagedResult<UserDto>> GetAllAsync(UserPageRequest request, CancellationToken ct)
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
        var url = string.IsNullOrEmpty(qs) ? "api/users" : $"api/users?{qs}";

        return await api.GetAsync<PagedResult<UserDto>>(url, ct);
    }

    public async Task<UserDto> GetByIdAsync(string id, CancellationToken ct)
    {
        return await api.GetAsync<UserDto>($"api/users/{id}", ct);
    }
}
