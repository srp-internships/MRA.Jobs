using Microsoft.Extensions.Options;
using MRA.Jobs.Application.Common.Models;

namespace MRA.Jobs.Infrastructure.Services;

public class GenericSmsService : ISmsService
{
    private readonly GenericSmsSettings _genericSmsSettings;

    public GenericSmsService(IOptions<GenericSmsSettings> genericSmsSettings)
    {
        _genericSmsSettings = genericSmsSettings.Value;
    }

    public async Task<string> SendSmsAsync(SmsMessage message)
    {
        const string smsSendingNumber = "+992123456789";
        return await Task.FromResult(Create(smsSendingNumber, message.Message, message.PhoneNumber));
    }

    private string Create(string from, string body, string to)
    {
        string result = $"To:   {to} /n" +
                        $"Body: {body} /n" +
                        $"From: {from} /n";
        return result;
    }
}

public class GenericSmsSettings
{
    public string AccountSid { get; set; }
    public string AuthToken { get; set; }
    public string FromNumber { get; set; }
}