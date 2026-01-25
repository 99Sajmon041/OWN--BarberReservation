using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Models.User.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Queries.Admin.GetUserById;

public sealed class GetUserByIdQueryHandler(
    ILogger<GetUserByIdQueryHandler> logger,
    UserManager<ApplicationUser> userManager,
    IMapper mapper) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = await userManager.FindByIdAsync(request.Id);
        if (user is null)
        {
            logger.LogWarning("User with ID: {UserId} was not found.", request.Id);
            throw new NotFoundException("Uživatel nebyl nalezen.");
        }

        var userDto = mapper.Map<UserDto>(user);
        userDto.Roles = (await userManager.GetRolesAsync(user)).ToList();

        logger.LogInformation("Admin fetched user detail. UserId: {UserId}", user.Id);

        return userDto;
    }
}
