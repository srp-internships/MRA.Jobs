using Mra.Shared.Common.Interfaces.Services;

namespace MRA.Jobs.Application.IntegrationTests.Common.Services;

public static class FakeEmailServiceReceiver
{
    public static string Body;
}

public class FakeEmailService:IEmailService
{
    public Task<bool> SendEmailAsync(IEnumerable<string> receives, string body, string subject)
    {
        FakeEmailServiceReceiver.Body = body;
        return Task.FromResult(true);
    }
}