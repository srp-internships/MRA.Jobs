namespace MRA.Identity.Application.Common.Interfaces.Services;
public interface IEmailService
{
    public Task<bool> SendEmail(string emailTo, string body, string subject);
}
    