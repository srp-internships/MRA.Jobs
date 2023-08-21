using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Infrastructure.Persistence;
using Newtonsoft.Json;

namespace AuthController.IntegrationTest;

public class LoginTest : BaseTest
{
    
    [OneTimeSetUp]
    public async Task Setup1() 
    {
        // Register a new user and add it to the database to check whether he has logged in
        var request1 = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = "password@#12P",
            FirstName = "Alex",
            Username = "@Alex22",
            LastName = "Makedonskiy",
            PhoneNumber = "123456789"
        };
        var content = new StringContent(JsonConvert.SerializeObject(request1), Encoding.UTF8, "application/json");
        
        await _client.PostAsync("api/Auth/register", content);
    }

    [Test]
    public async Task Login_RequestWithCorrectLoginData_ReturnsOk()
    {
        // Arrange
        var request = new LoginUserCommand {Username = "@Alex22", Password = "password@#12P"};

        // Act
        var response = await _client.PostAsJsonAsync("api/Auth/login", request);
        var jwt = await response.Content.ReadFromJsonAsync<JwtTokenResponse>();
        var result = jwt?.AccessToken;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result, Is.Not.Null);
        });
    }

    [Test]
    public async Task Login_RequestWithEmptyLoginData_ReturnsBadRequest()
    {
        // Arrange
        var request = new LoginUserCommand { };
        
        // Act
        var response = await _client.PostAsJsonAsync("api/Auth/login", request);
        var jwt = await response.Content.ReadFromJsonAsync<JwtTokenResponse>();
        var result = jwt?.AccessToken;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(result, Is.Null);
        });
    }

    [Test]
    [TestCase("@Alex22","password")]
    [TestCase("@Alex22","")]
    [TestCase("@Alex","pass")]
    [TestCase("@Alex","password@#12P")]
    [TestCase("","")]
    [TestCase(null,null)]
    public async Task Login_RequestWithIncorrectLoginData_ReturnsBadRequest(string? username, string? password)
    {
        // Arrange
        var request = new LoginUserCommand {Username = username, Password = password};
        
        // Act
        var response = await _client.PostAsJsonAsync("api/Auth/login", request);
        
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}