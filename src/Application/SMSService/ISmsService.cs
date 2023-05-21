namespace MRA.Jobs.Application.SMSService;

public interface ISmsService
{
    Task<string> SendSmsAsync(SmsMessage message);
}