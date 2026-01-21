using AutoMapper;
using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace BarberReservation.Application.User.Commands.Admin.CreateHairdresser;

public sealed class CreateHairdresserCommandHandler(
    ILogger<CreateHairdresserCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    IEmailService emailService,
    IMapper mapper) : IRequestHandler<CreateHairdresserCommand>
{
    public async Task<Unit> Handle(CreateHairdresserCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var hairdresser = mapper.Map<ApplicationUser>(request);

        hairdresser.IsActive = true;
        hairdresser.MustChangePassword = true;
        hairdresser.EmailConfirmed = true;
        hairdresser.UserName = hairdresser.Email;

        var tempPassword = GenerateTemporaryPassword();

        var createResult = await userManager.CreateAsync(hairdresser, tempPassword);
        if (!createResult.Succeeded)
        {
            var error = createResult.Errors.Select(e => e.Description);

            logger.LogError("Vytvoření kadeřníka selhalo. Errors: {Errors}", error);
            throw new BarberReservation.Application.Exceptions.ValidationException("Chyba: " + error);
        }

        var roleResult = await userManager.AddToRoleAsync(hairdresser, nameof(UserRoles.Hairdresser));
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(hairdresser);

            var error = createResult.Errors.Select(e => e.Description);
            
            logger.LogError("Vytvoření role kadeřníkovi selhalo. Errors: {Errors}", error);
            throw new BarberReservation.Application.Exceptions.ValidationException("Chyba: " + error);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(hairdresser);
        await emailService.SendPasswordResetEmailAsync(hairdresser.Email!, token, ct);

        logger.LogInformation("Hairdresser {HairdresserId} ({Email}) created. Reset email sent.", hairdresser.Id, hairdresser.Email);

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
