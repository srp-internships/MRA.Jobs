using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MRA.Jobs.Application.Common.Models;

namespace MRA.Jobs.Infrastructure.Services;
public class SmtpEmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IOptions<SmtpSettings> smtpSettings, ILogger<SmtpEmailService> logger)
    {
        _smtpSettings = smtpSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(EmailMessage message)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(message.From));
            email.To.Add(MailboxAddress.Parse(message.To));
            email.Subject = message.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_smtpSettings.Server, _smtpSettings.Port, false);
            smtp.Authenticate(_smtpSettings.Username, _smtpSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"Argument Null Exception: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Invalid Operation Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception: {ex.Message}");
        }
    }

}

public class SmtpSettings
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; }
}
