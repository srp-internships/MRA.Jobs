using System.Net;
using System.Net.Http.Json;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;
using Mra.Shared.Services;

namespace MRA.Jobs.Application.IntegrationTests;

[TestFixture]
public class EmailTest : BaseTest
{
    [Test]
    public async Task Email_VerifyEmail_True()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "test1@example.com",
            Password = "password@#12P",
            ConfirmPassword = "password@#12P",
            FirstName = "Alex",
            Username = "@Alex221",
            LastName = "Makedonsky1",
            PhoneNumber = "123456789"
        };

        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        var splitted = SendEmailData.Body.Split("'")[1].Split("token=")[1];

        var requestLogin = new LoginUserCommand { Username = "@Alex221", Password = "password@#12P" };

        // Act
        var responseLogin = await _client.PostAsJsonAsync("api/Auth/login", requestLogin);
        var jwt = await responseLogin.Content.ReadFromJsonAsync<JwtTokenResponse>();


        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt.AccessToken);

        var responseEmail = await _client.GetAsync($"api/Auth/verify?token={WebUtility.UrlEncode(splitted)}");

        var stringres = responseEmail.Content.ReadAsStringAsync();

        // Assert
        
        responseEmail.EnsureSuccessStatusCode();
       // Assert.That(responseEmail.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var user = await GetEntity<ApplicationUser>(s => s.Email == request.Email);
        Assert.IsTrue(user.EmailConfirmed);
    }

    [Test]
    public async Task Email_VerifyEmail_BadRequest()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "test1@example.com",
            Password = "password@#12P",
            ConfirmPassword = "password@#12P",
            FirstName = "Alex",
            Username = "@Alex221",
            LastName = "Makedonsky1",
            PhoneNumber = "123456789"
        };
        // Act
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        var splitted = "aaaa1";

        var requestLogin = new LoginUserCommand { Username = "@Alex221", Password = "password@#12P" };

        // Act
        var responseLogin = await _client.PostAsJsonAsync("api/Auth/login", requestLogin);
        var jwt = await responseLogin.Content.ReadFromJsonAsync<JwtTokenResponse>();


        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt.AccessToken);


        var responseEmail = await _client.GetAsync($"api/Auth/verify?token={WebUtility.UrlEncode(splitted)}");
        // Assert

        Assert.That(responseEmail.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var user = await GetEntity<ApplicationUser>(s => s.Email == request.Email);
        Assert.IsFalse(user.EmailConfirmed);
    }
}