using BarberReservation.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BarberReservation.Infrastructure.Email;

public sealed class EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger) : IEmailService
{
    public async Task SendPasswordResetEmailAsync(string email, string token, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var setting = options.Value;
        var resetUrl = $"{setting.FrontendBaseUrl}/reset-password?email={WebUtility.UrlEncode(email)}&token={WebUtility.UrlEncode(token)}";

        var subject = "Obnovení hesla: Barber-Shop";

        var bodyBuilder = new StringBuilder();
        bodyBuilder.AppendLine("<p>Dobrý den,</p>");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine("<p>Posíláme Vá informace k obnovení hesla k Vašemu účtu v Barber-Shopu.</p>");
        bodyBuilder.AppendLine("<p>Pro změnu hesla klikněte na tento odkaz:</p>");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine($"<p><a href='{resetUrl}'>Obnovit heslo</a></p>");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine("<p>Pokud jste o změnu hesla nežádali / nebo nemáte nově založený účet, tento e-mail prosím ignorujte.</p>");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine("<p>S pozdravem<br/>Tým Barber-shop</p>");

        using var mailMessage = new MailMessage
        {
            From = new MailAddress(setting.FromAddress, setting.FromName),
            Subject = subject,
            Body = bodyBuilder.ToString(),
            IsBodyHtml = true
        };

        mailMessage.To.Add(email);

        using var smtpClient = new SmtpClient(setting.SmtpHost, setting.SmtpPort)
        {
            EnableSsl = setting.EnableSsl,
            Credentials = new NetworkCredential(setting.UserName, setting.Password)
        };

        try
        {
            logger.LogInformation("Sending password reset to email: {Email}.", email);

            await smtpClient.SendMailAsync(mailMessage, cancellationToken);

            logger.LogInformation("Password reset email sent to: {Email}.", email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send password reset email to: {Email}.", email);
            throw;
        }
    }
}
