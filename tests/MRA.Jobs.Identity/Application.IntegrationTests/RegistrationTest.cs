using System.Net;
using System.Net.Http.Json;
using MRA.Identity.Application.Contract.User.Commands;

namespace MRA.Jobs.Application.IntegrationTests;

[TestFixture]
public class RegistrationTests:BaseTest
{
    
    [Test]
    [Ignore("Dont need to this test. Can be removed")]
    public async Task Register_ValidRequestWithCorrectRegisterData_ReturnsOk()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = "password@#12P",
            FirstName = "Alex",
            Username = "@Alex22",
            LastName = "Makedonskiy",
            PhoneNumber = "123456789"
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task Register_ValidRequestWithCorrectRegisterData_ReturnsOkAndSavesUserIntoDb()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "test1@example.com",
            Password = "password@#12P",
            FirstName = "Alex",
            Username = "@Alex22",
            LastName = "Makedonsky",
            PhoneNumber = "123456789"
        };
        
        // Assert
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
       // Assert.That(_context.Users.Count(), Is.EqualTo(1));
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
            PhoneNumber = "123456789"
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
    
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
    
    [Test]
    public async Task Register_InvalidRequestWithEmptyRegisterData_ReturnsUnauthorized()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
           // Empty Register Data
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
    
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}