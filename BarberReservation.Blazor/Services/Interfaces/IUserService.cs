using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.LookUpModels;
using BarberReservation.Shared.Models.User.Admin;
using BarberReservation.Shared.Models.User.Common;

namespace BarberReservation.Blazor.Services.Interfaces;

public interface IUserService
{
    Task<PagedResult<UserDto>> GetAllAsync(UserPageRequest request, CancellationToken ct);
    Task<List<GetLookUpHairdressers>> GetAvailableHairdressersAsync(int reservationId, CancellationToken ct);
    Task<UserDto> GetByIdAsync(string id, CancellationToken ct);
    Task DeactivateByIdAsync(string id, CancellationToken ct);
    Task ActivateByIdAsync(string id, CancellationToken ct);
    Task<CreateUserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct);
    Task UpdateAsync(string id, UpdateUserRequest request, CancellationToken ct);
    Task UpdateEmailAsync(string id, UpdateUserEmailRequest request, CancellationToken ct);
    Task<UserDto> GetProfileAsync(CancellationToken ct);
}
