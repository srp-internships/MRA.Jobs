using Azure.Communication.Email;
using Azure.Communication.Email.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MRA.Identity.Application;
using MRA.Identity.Application.Common.Interfaces.Services;

namespace MRA.Identity.Infrastructure.Account.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly EmailClient _client;
    private readonly ILogger _logger;

    public EmailService(IConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        var connectionString =
            _configuration[
                ApplicationConstants
                    .AZURE_EMAIL_CONNECTION]; // Find your Communication Services resource in the Azure portal
        _client = new EmailClient(connectionString);
        _logger = logger;
    }

    public async Task<bool> SendEmail(string emailTo, string body, string subject)
    {
        // Create the email content
        var emailContent = new EmailContent(subject) { Html = body };

        // Create the recipient list
        var emailRecipients = new EmailRecipients(
            new List<EmailAddress> { new(emailTo) });

        // Create the EmailMessage
        var emailMessage = new EmailMessage(
            _configuration[ApplicationConstants.AZURE_EMAIL_SENDER],
            emailContent,
            emailRecipients);

        try
        {
            await _client.SendAsync(emailMessage);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on sending email");
            return false;
        }
    }
}