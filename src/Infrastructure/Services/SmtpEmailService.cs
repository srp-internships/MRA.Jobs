using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using MRA.Jobs.Application.Common.Models;

namespace MRA.Jobs.Infrastructure.Services;
public class SmtpEmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly SmtpClient _smtpClient;

    public SmtpEmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;

        _smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
        {
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = _smtpSettings.EnableSsl
        };
    }

    public async Task SendEmailAsync(EmailMessage message)
    {
        var mailMessage = new MailMessage(message.From, message.To, message.Subject, message.Body)
        {
            IsBodyHtml = message.IsHtml
        };

        await _smtpClient.SendMailAsync(mailMessage);
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
