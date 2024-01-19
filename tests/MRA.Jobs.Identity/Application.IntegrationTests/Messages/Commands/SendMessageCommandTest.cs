using System.Net;
using System.Net.Http.Json;

using MRA.Identity.Application.Contract.Messages.Commands;

namespace MRA.Jobs.Application.IntegrationTests.Messages.Commands;
public class SendMessageCommandTest : BaseTest
{
    [Test]
    public async Task SendingMessage_Success()
    {
        await AddAuthorizationAsync();

        var command = new SendMessageCommand
        {
            Phone = "988888888",
            Text = "Hello world",
        };

        var response = await _client.PostAsJsonAsync("api/message", command);
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task SendingMessage_UnauthorizedForDefaultUser()
    {
        await AddApplicantAuthorizationAsync();

        var command = new SendMessageCommand
        {
            Phone = "988888888",
            Text = "Hello world",
        };

        var response = await _client.PostAsJsonAsync("api/message", command);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
    [Test]
    public async Task SendingMessage_WrongPhoneNumberFormat()
    {
        await AddAuthorizationAsync();

        var command = new SendMessageCommand
        {
            Phone = "8888888",
            Text = "Hello world",
        };

        var response = await _client.PostAsJsonAsync("api/message", command);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

}
