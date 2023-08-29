using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mra.Shared.Common.Interfaces.Services;

namespace MRA.Jobs.Application.IntegrationTests.Email;
public static class TestEmailSendbox
{
    public static string Body { get ; set; }    
}
public class TestEmailSenderService : IEmailService
{
    public Task<bool> SendEmailAsync(IEnumerable<string> receives, string body, string subject)
    {
       TestEmailSendbox.Body = body;
        return Task.FromResult(true);
    }
}
