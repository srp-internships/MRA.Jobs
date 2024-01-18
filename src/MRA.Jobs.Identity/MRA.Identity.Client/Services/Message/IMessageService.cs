using MRA.Identity.Application.Contract.Messages.Commands;

namespace MRA.Identity.Client.Services.Message;

public interface IMessageService
{
    Task<HttpResponseMessage> SendMessageAsync(SendMessageCommand command);
}
