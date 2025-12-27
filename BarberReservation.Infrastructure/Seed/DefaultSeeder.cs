using BarberReservation.Domain.Entities;
using BarberReservation.Infrastructure.Options;
using BarberReservation.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BarberReservation.Infrastructure.Seed;

public sealed class DefaultSeeder(IOptions<DefaultUser> options, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    public async Task SeedDefaultData()
    {
        var defaultUser = options.Value;

        foreach (var roleName in Roles())
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    LogErrors("Create role failed", roleResult.Errors);
                    return;
                }
            }
        }

        var user = await userManager.FindByEmailAsync(defaultUser.Email);

        if (user is null)
        {
            user = new ApplicationUser
            {
                FirstName = defaultUser.FirstName,
                LastName = defaultUser.LastName,
                UserName = defaultUser.Email,
                Email = defaultUser.Email,
                IsActive = true
            };

            var createResult = await userManager.CreateAsync(user, defaultUser.Password);
            if (!createResult.Succeeded)
            {
                LogErrors("Create user failed", createResult.Errors);
                return;
            }
        }

        if (!await userManager.IsInRoleAsync(user, defaultUser.Role))
        {
            var addRoleResult = await userManager.AddToRoleAsync(user, defaultUser.Role);
            if (!addRoleResult.Succeeded)
            {
                LogErrors("Add to role failed", addRoleResult.Errors);
                return;
            }
        }
    }

    private static IEnumerable<string> Roles()
    {
        return
        [
            UserRoles.Customer.ToString(),
            UserRoles.Admin.ToString(),
            UserRoles.Hairdresser.ToString()
        ];
    }


    private static void LogErrors(string prefix, IEnumerable<IdentityError> errors)
    {
        var msg = string.Join(", ", errors.Select(e => e.Description));
        Console.WriteLine($"{prefix}: {msg}");
    }
}
