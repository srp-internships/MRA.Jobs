using MRA.Jobs.Application.Common.Models;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface ISmsService
{
    Task<string> SendSmsAsync(SmsMessage message);
}