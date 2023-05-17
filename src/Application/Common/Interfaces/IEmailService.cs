using MRA.Jobs.Application.Common.Models;

namespace MRA.Jobs.Application.Common.Interfaces;
public interface IEmailService
{
    Task SendEmailAsync(EmailMessage message);
}
