using System.Net;
using System.Net.Http.Json;
using MRA.Identity.Application.Contract.User.Commands.ChangePassword;

namespace MRA.Jobs.Application.IntegrationTests.Users.Command;
public class ChangeUserPasswordTest : BaseTest
{
    [Test]
    public async Task ChangePassword_ReturnsOk()
    {
        await AddApplicantAuthorizationAsync();

        // Arrange
        var command = new ChangePasswordUserCommand
        {
            OldPassword = "password@#12P",
            NewPassword = "password@#12P123",
            ConfirmPassword = "password@#12P123"
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/Auth/ChangePassword", command);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Test]
    public async Task ChangePassword_IncorrectPassword()
    {
        await AddApplicantAuthorizationAsync();

        // Arrange
        var command = new ChangePasswordUserCommand
        {
            OldPassword = "password@#12P11111111",
            NewPassword = "password@#12P123",
            ConfirmPassword = "password@#12P123"
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/Auth/ChangePassword", command);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        // var responseString = (await response.Content.ReadFromJsonAsync<ProblemDetails>()).Detail;
        // Assert.AreEqual("Incorrect old password", responseString);
    }
}
