using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace BarberReservation.Application.User.Commands.Admin.CreateUser;

public sealed class CreateUserCommandHandler(
    ILogger<CreateUserCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    IEmailService emailService,
    IMapper mapper) : IRequestHandler<CreateUserCommand>
{
    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if(request.Role == nameof(UserRoles.Admin))
        {
            logger.LogWarning("Admin tries create user with role Admin. Not allowed - admin is system role.");
            throw new ConflictException("Nelze vytvořit roli Admina - ta je systémově daná.");
        }

        var user = mapper.Map<ApplicationUser>(request);

        user.IsActive = true;
        user.MustChangePassword = true;
        user.EmailConfirmed = true;
        user.UserName = user.Email;

        var tempPassword = GenerateTemporaryPassword();

        var createResult = await userManager.CreateAsync(user, tempPassword);
        if (!createResult.Succeeded)
        {
            var error = string.Join(", ", createResult.Errors.Select(e => e.Description));

            logger.LogError("Vytvoření kadeřníka selhalo. Errors: {Errors}", error);
            throw new ValidationException("Chyba: " + error);
        }

        var roleResult = await userManager.AddToRoleAsync(user, request.Role);
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);

            var error = string.Join(", ", roleResult.Errors.Select(e => e.Description));

            logger.LogError("Role create for user failed. Errors: {Errors}", error);
            throw new ValidationException("Chyba: " + error);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        await emailService.SendPasswordResetEmailAsync(user.Email!, token, ct);

        logger.LogInformation("User {UserId} ({Email}) created. Reset email sent.", user.Id, user.Email);

        return Unit.Value;
    }

    private static string GenerateTemporaryPassword(int length = 16)
    {
        const string lower = "abcdefghijkmnopqrstuvwxyz";
        const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string digits = "23456789";
        const string symbols = "!@$?_-";

        var chars = new List<char>
        {
            lower[RandomNumberGenerator.GetInt32(lower.Length)],
            upper[RandomNumberGenerator.GetInt32(upper.Length)],
            digits[RandomNumberGenerator.GetInt32(digits.Length)],
            symbols[RandomNumberGenerator.GetInt32(symbols.Length)]
        };

        var all = lower + upper + digits + symbols;

        while (chars.Count < length)
        {
            chars.Add(all[RandomNumberGenerator.GetInt32(all.Length)]);
        }

        for (int i = chars.Count - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }

        return new string(chars.ToArray());
    }
}
