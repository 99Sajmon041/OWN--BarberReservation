using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.User.Admin;
using BarberReservation.Shared.Models.User.Common;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface IUserService
{
    Task<PagedResult<UserDto>> GetAllAsync(UserPageRequest request, CancellationToken ct);
    Task<UserDto> GetByIdAsync(string id, CancellationToken ct);
}
