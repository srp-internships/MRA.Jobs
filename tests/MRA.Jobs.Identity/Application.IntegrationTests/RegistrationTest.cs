using System.Net;
using System.Net.Http.Json;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests;

[TestFixture]
public class RegistrationTests : BaseTest
{
    [Test]
    public async Task Register_ValidRequestWithCorrectRegisterData_ReturnsOkAndSavesUserIntoDb()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "test3@example.com",
            Password = "password@#12P",
            ConfirmPassword = "password@#12P",
            FirstName = "Alex",
            Username = "@Alex223",
            LastName = "Makedonsky",
            PhoneNumber = "123456789"
        };

        // Assert
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        // Assert
        var registeredUser = await GetEntity<ApplicationUser>(u => u.Email == request.Email && u.UserName == request.Username);
        Assert.IsNotNull(registeredUser, "Registered user not found");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Register_InvalidRequestWithWrongRegisterData_ReturnsUnauthorized()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "test1@example.com",
            Password = "password", // incorrect password            
            FirstName = "Alex",
            Username = "@Alex22",
            LastName = "Makedonskiy",
            PhoneNumber = "123456789",
            ConfirmPassword="password"
        };

        // Assert
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Register_InvalidRequestWithEmptyRegisterData_ReturnsUnauthorized()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            // Empty Register Data
            Password= "password",
            ConfirmPassword= "password",
        };

        // Assert
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized),await response.Content.ReadAsStringAsync());
    }
}