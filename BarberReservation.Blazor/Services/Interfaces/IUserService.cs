using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.User.Admin;
using BarberReservation.Shared.Models.User.Common;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface IUserService
{
    Task<PagedResult<UserDto>> GetAllAsync(UserPageRequest request, CancellationToken ct);
    Task<UserDto> GetByIdAsync(string id, CancellationToken ct);
    Task DeactivateByIdAsync(string id, CancellationToken ct);
    Task ActivateByIdAsync(string id, CancellationToken ct);
    Task CreateUserAsync(CreateUserRequest request, CancellationToken ct);
    Task UpdateUserAsync(string id, UpdateUserRequest request, CancellationToken ct);
    Task UpdateUserEmailAsync(string id, UpdateUserEmailRequest request, CancellationToken ct);
}
