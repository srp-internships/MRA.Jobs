using MRA.Identity.Application.Contract.Messages.Commands;
using MRA.Identity.Application.Contract.Messages.Responses;

namespace MRA.Identity.Client.Services.Message;

public interface IMessageService
{
    Task<HttpResponseMessage> SendMessageAsync(SendMessageCommand command);
    Task<List<GetMessageResponse>> GetAllMessagesAsync();
}
