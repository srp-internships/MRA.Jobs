using System.Net.Http.Json;
using MRA.Identity.Application.Contract.User.Commands.ResetPassword;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Users.Command;
public class ResetPasswordCommandTest : BaseTest
{
    [Test]
    public async Task ResetPasswordTest_ReturnsSuccess()
    {
        await AddAuthorizationAsync();
        var query = new SendVerificationCodeSmsQuery { PhoneNumber = "+992123456789" };

        var response = await _client.GetAsync($"api/sms/send_code?PhoneNumber={Uri.EscapeDataString(query.PhoneNumber)}");
        int code = (await GetEntity<ConfirmationCode>(c => c.PhoneNumber == query.PhoneNumber)).Code;

        var command = new ResetPasswordCommand()
        {
            PhoneNumber = query.PhoneNumber,
            Code = code,
            Password = "password@#12P"
        };

        var result = await _client.PostAsJsonAsync("/api/Auth/ResetPassword", command);

        response.EnsureSuccessStatusCode();
    }
}
