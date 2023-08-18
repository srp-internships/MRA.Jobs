namespace MRA.Identity.Application.Common.Interfaces.Services;
public interface IEmailService
{
    public bool SendEmail(string emailTo, string body, string subject);
}
