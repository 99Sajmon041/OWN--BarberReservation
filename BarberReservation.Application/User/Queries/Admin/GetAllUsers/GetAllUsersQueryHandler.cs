using AutoMapper;
using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.User.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Queries.Admin.GetAllUsers;

public sealed class GetAllUsersQueryHandler(
    ILogger<GetAllUsersQueryHandler> logger,
    UserManager<ApplicationUser> userManager,
    IMapper mapper) : IRequestHandler<GetAllUsersQuery, PagedResult<UserDto>>
{
    public async Task<PagedResult<UserDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        var query = userManager.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            var usersInRole = await userManager.GetUsersInRoleAsync(request.Role);

            var idSet = usersInRole.Select(x => x.Id).ToHashSet();
            query = query.Where(x => idSet.Contains(x.Id));
        }

        if (request.IsActive is true)
            query = query.Where(x => x.IsActive);
        else if (request.IsActive is false)
            query = query.Where(x => !x.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(u =>
                u.FirstName.Contains(s) ||
                u.LastName.Contains(s) ||
                u.Email!.Contains(s) ||
                u.PhoneNumber!.Contains(s));
        }

        query = ApplySort(query, request.SortBy, request.Desc);

        var totalItemsCount = await query.CountAsync(ct);

        var users = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var usersDtos = mapper.Map<List<UserDto>>(users);

        logger.LogInformation("Admin fetched all filtered users to list - {CountOfUsers} records", totalItemsCount);

        return new PagedResult<UserDto>
        {
            Items = usersDtos,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItemsCount = totalItemsCount
        };
    }

    private static IQueryable<ApplicationUser> ApplySort(IQueryable<ApplicationUser> query, string? sortBy, bool desc)
    {
        var key = (sortBy ?? "").Trim().ToLowerInvariant();

        return key switch
        {
            "firstname" => desc ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),

            "lastname" => desc ? query.OrderByDescending(u => u.LastName).ThenByDescending(u => u.FirstName) : query.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),

            "email" => desc ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),

            "phonenumber" => desc ? query.OrderByDescending(u => u.PhoneNumber) : query.OrderBy(u => u.PhoneNumber),

            "isactive" => desc ? query.OrderByDescending(u => u.IsActive).ThenBy(u => u.LastName) : query.OrderBy(u => u.IsActive).ThenBy(u => u.LastName),

            _ => query.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
        };
    }

}
