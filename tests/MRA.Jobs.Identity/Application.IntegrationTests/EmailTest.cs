using System.Net;
using System.Net.Http.Json;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;
using MRA.Jobs.Application.IntegrationTests.Email;

namespace MRA.Jobs.Application.IntegrationTests;

[TestFixture]
public class EmailTest : BaseTest
{
    [Test]
    public async Task VerifyEmail()
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

        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        var splitted = TestEmailSendbox.Body.Split("'")[1].Split("token=")[1];

        var requestLogin = new LoginUserCommand { Username = "@Alex22", Password = "password@#12P" };

        // Act
        var responseLogin = await _client.PostAsJsonAsync("api/Auth/login", requestLogin);
        var jwt = await responseLogin.Content.ReadFromJsonAsync<JwtTokenResponse>();


        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt.AccessToken);

        var responseEmal = await _client.GetAsync($"api/Auth/verify?token={splitted}");

        // Assert

        Assert.That(responseEmal.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var user = await GetEntity<ApplicationUser>(s => s.Email == request.Email);
        Assert.IsTrue(user.EmailConfirmed);
    }

    [Test]
    public async Task Verif()
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
        // Act
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        var splitted = "aaaa1";
        var responseEmal = await _client.GetAsync($"api/Auth/verify/{splitted}");
        // Assert

        Assert.That(responseEmal.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var user = await GetEntity<ApplicationUser>(s => s.Email == request.Email);
        Assert.IsFalse(user.EmailConfirmed);
    }
}