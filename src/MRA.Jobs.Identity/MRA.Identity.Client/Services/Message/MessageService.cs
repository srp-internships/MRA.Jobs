using System.Net.Http;
using System.Net.Http.Json;
using MRA.Identity.Application.Contract.Messages.Commands;
using MRA.Identity.Application.Contract.Messages.Responses;

namespace MRA.Identity.Client.Services.Message;

public class MessageService : IMessageService
{
    private readonly HttpClient _httpClient;

    public MessageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<GetMessageResponse>> GetAllMessagesAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<GetMessageResponse>>("message");
        return result;
    }

    public async Task<HttpResponseMessage> SendMessageAsync(SendMessageCommand command)
    {
        var result = await _httpClient.PostAsJsonAsync("message", command);
        return result;
    }
}
