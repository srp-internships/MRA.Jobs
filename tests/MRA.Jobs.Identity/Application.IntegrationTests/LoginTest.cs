using System.Net;
using System.Net.Http.Json;
using MRA.Identity.Application.Contract.Application.Responses;
using MRA.Identity.Application.Contract.User.Commands;

namespace MRA.Jobs.Application.IntegrationTests;

[TestFixture]
public class LoginTest : BaseTest
{
    [Test]
    public async Task Login_RequestWithCorrectLoginData_ReturnsOk()
    {
        // Arrange
        var request = new LoginUserCommand {Username = "@Alex33", Password = "password@#12P"};

        // Act
        var response = await _client.PostAsJsonAsync("api/Auth/login", request);
        
        // Assert
        var jwt = await response.Content.ReadFromJsonAsync<JwtTokenResponse>();
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(jwt?.AccessToken, Is.Not.Null.Or.Empty);
        });
    }

    [Test]
    public async Task Login_RequestWithEmptyLoginData_ReturnsUnauthorized()
    {
        // Arrange
        var request = new LoginUserCommand {Username = "null", Password = "null"};

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/login", request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        });
    }

    [Test]
    [TestCase("@Alex22", "password")]
    [TestCase("@Alex22", "")]
    [TestCase("@Alex", "pass")]
    [TestCase("@Alex", "password@#12P")]
    [TestCase("", "")]
    public async Task Login_RequestWithIncorrectLoginData_ReturnsUnauthorized(string username, string password)
    {
        // Arrange
        var request = new LoginUserCommand {Username = username, Password = password};

        // Act
        var response = await _client.PostAsJsonAsync("api/Auth/login", request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}