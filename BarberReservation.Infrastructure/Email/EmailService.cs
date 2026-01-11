using BarberReservation.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BarberReservation.Infrastructure.Email;

public sealed class EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger) : IEmailService
{
    public async Task SendPasswordResetEmailAsync(string email, string token, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var setting = options.Value;
        var resetUrl = $"{setting.FrontendBaseUrl}/reset-password?email={WebUtility.UrlEncode(email)}&token={WebUtility.UrlEncode(token)}";

        var subject = "Obnovení hesla: Barber-Shop";

        var bodyBuilder = new StringBuilder()
            .AppendLine("<p>Dobrý den,</p>")
            .AppendLine()
            .AppendLine("<p>Posíláme Vám informace k obnovení hesla k Vašemu účtu v Barber-Shopu.</p>")
            .AppendLine("<p>Pro změnu hesla klikněte na tento odkaz:</p>")
            .AppendLine()
            .AppendLine($"<p><a href='{resetUrl}'>Obnovit heslo</a></p>")
            .AppendLine()
            .AppendLine("<p>Pokud jste o změnu hesla nežádali nebo nemáte nově založený účet, tento e-mail prosím ignorujte.</p>")
            .AppendLine()
            .AppendLine("<p>S pozdravem<br/>Tým Barber-shop</p>");

        await SendEmailAsync(email, subject, bodyBuilder.ToString(), ct);
    }

    public async Task SendReservationConfirmationEmailAsync(
        string email,
        DateTime reservationTime,
        string hairdresserName,
        string serviceName,
        decimal price,
        int durationTime,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var subject = "Potvrzení objednávky: Barber-Shop";

        var bodyBuilder = new StringBuilder()
            .AppendLine("<p>Dobrý den,</p>")
            .AppendLine()
            .AppendLine("<p>Posíláme Vám informace k Vaší rezervaci v Barber-Shopu.</p>")
            .AppendLine()
            .AppendLine($"<p><b>Název služby:</b> {WebUtility.HtmlEncode(serviceName)}</p>")
            .AppendLine($"<p><b>Jméno kadeřníka:</b> {WebUtility.HtmlEncode(hairdresserName)}</p>")
            .AppendLine($"<p><b>Cena služby:</b> {price.ToString("C2")}</p>")
            .AppendLine($"<p><b>Délka služby:</b> {durationTime} minut</p>")
            .AppendLine($"<p><b>Datum rezervace:</b> {reservationTime:dd.MM.yyyy HH:mm}</p>")
            .AppendLine()
            .AppendLine("<p><i>V případě zrušení rezervace méně než 24 hodin předem Vám může být účtován poplatek.</i></p>")
            .AppendLine()
            .AppendLine("<p>Budeme se na Vás těšit!</p>")
            .AppendLine("<p>S pozdravem<br/>Tým Barber-shop</p>");

        await SendEmailAsync(email, subject, bodyBuilder.ToString(), ct);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string htmlBody, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var setting = options.Value;

        using var mailMessage = new MailMessage
        {
            From = new MailAddress(setting.FromAddress, setting.FromName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        using var smtpClient = new SmtpClient(setting.SmtpHost, setting.SmtpPort)
        {
            EnableSsl = setting.EnableSsl,
            Credentials = new NetworkCredential(setting.UserName, setting.Password)
        };

        try
        {
            logger.LogInformation("Sending email '{Subject}' to: {Email}.", subject, toEmail);

            await smtpClient.SendMailAsync(mailMessage, ct);

            logger.LogInformation("Email '{Subject}' sent to: {Email}.", subject, toEmail);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email '{Subject}' to: {Email}.", subject, toEmail);
            throw;
        }
    }
}
