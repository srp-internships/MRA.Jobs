using System.Net.Http.Json;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Users.Query;

[TestFixture]
public class SendVerificationCodeSmsQueryTest : BaseTest
{
    [Test]
    public async Task SendVerificationCodeSmsQueryHandler_SendingSmsWithCorrectPhoneNumber_Success()
    {
        // Arrange
        await AddAuthorizationAsync();
        var query = new SendVerificationCodeSmsQuery { PhoneNumber = "911111111" };

        // Act
        var response = await _client.GetAsync($"api/sms/send_code?PhoneNumber={query.PhoneNumber}");
        response.EnsureSuccessStatusCode();

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Multiple(() =>
        {
            Assert.AreEqual("true", responseContent, "Expected response content to be 'true'.");
            Assert.NotNull(TestSmsSandbox.PhoneNumber);
            Assert.NotNull(TestSmsSandbox.Text);
        });
    }

    [Test]
    public async Task SmsVerificationCodeCheckQueryHandler_VerifyingSmsCode_Success()
    {
        // Arrange
        await AddAuthorizationAsync();
        var query = new SendVerificationCodeSmsQuery { PhoneNumber = "+992123456789" };
        await _client.GetAsync($"api/sms/send_code?PhoneNumber={query.PhoneNumber}");

        int code = (await GetEntity<ConfirmationCode>(c => c.PhoneNumber == query.PhoneNumber)).Code;

        // Act
        var response = await _client.GetAsync($"api/sms/verify_code?PhoneNumber={query.PhoneNumber}&Code={code}");
        response.EnsureSuccessStatusCode();

        // Assert
        var responseContent = await response.Content.ReadFromJsonAsync<SmsVerificationCodeStatus>();


        Assert.AreEqual(SmsVerificationCodeStatus.CodeVerifySuccess, responseContent, "Expected response content to be 'true'.");
    }
}
