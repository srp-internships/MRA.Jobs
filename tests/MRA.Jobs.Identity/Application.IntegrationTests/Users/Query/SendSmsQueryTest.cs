using MRA.Identity.Application.Contract.User.Queries;

namespace MRA.Jobs.Application.IntegrationTests.Users.Query;

[TestFixture]
public class SendSmsQueryTest : BaseTest
{
    [Test]
    public async Task SendSmsQueryHandler_SendingSmsWithCorrectPhoneNumber_Success()
    {
        // Arrange
        var query = new SendSmsQuery { PhoneNumber = "931111111" };

        // Act
        var response = await _client.GetAsync($"api/sms/send_sms?PhoneNumber={query.PhoneNumber}");
        response.EnsureSuccessStatusCode();

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("true", responseContent, "Expected response content to be 'true'.");
    }
}
