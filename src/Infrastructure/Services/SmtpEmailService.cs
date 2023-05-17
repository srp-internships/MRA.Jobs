using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MRA.Jobs.Application.Common.Models;

namespace MRA.Jobs.Infrastructure.Services;
public class SmtpEmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly SmtpClient _smtpClient;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IOptions<SmtpSettings> smtpSettings, ILogger<SmtpEmailService> logger)
    {
        _smtpSettings = smtpSettings.Value;

        _smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
        {
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = _smtpSettings.EnableSsl
        };
        _logger = logger;
    }

    public async Task SendEmailAsync(EmailMessage message)
    {
        try
        {
            var mailMessage = new MailMessage(message.From, message.To, message.Subject, message.Body)
            {
                IsBodyHtml = message.IsHtml
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"ArgumentNull Exception: {ex.Message}");
        }
        catch (SmtpException ex)
        {
            _logger.LogError($"SMTP Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception: {ex.Message}");
        }
        finally
        {
            _smtpClient.Dispose();
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
