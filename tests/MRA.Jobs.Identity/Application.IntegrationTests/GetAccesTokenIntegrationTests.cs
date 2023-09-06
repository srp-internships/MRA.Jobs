using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using MRA.Identity.Application.Contract.Application.Responses;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Contract.User.Queries;

namespace MRA.Jobs.Application.IntegrationTests;

[TestFixture]
public class GetAccesTokenIntegrationTests : BaseTest
{
    private JwtTokenResponse loginResponce { get; set; }

    [SetUp]
    public async Task SetUp()
    {
        if (loginResponce == null)
        {
            var registerCommand1 = new RegisterUserCommand
            {
                Email = "test1@example.com",
                Password = "password@#12P",
                FirstName = "Alex",
                Username = "@Alex22",
                LastName = "Makedonsky",
                PhoneNumber = "123456789"
            };
            var loginCommand1 = new LoginUserCommand
            {
                Username = "@Alex22",
                Password = "password@#12P"
            };

            await RegisterUser(registerCommand1);
            loginResponce = await LoginUser(loginCommand1);
        }
    }

    [Test]
    public async Task Refresh_ValidRefreshAndAccessToken_OkResult()
    {
        //Arrange
        var request = new GetAccesTokenUsingRefreshTokenQuery()
        {
            AccessToken = loginResponce.AccessToken,
            RefreshToken = loginResponce.RefreshToken
        };

        //Act
        var responce = await _client.PostAsJsonAsync("api/Auth/refresh", request);

        //Assert
        responce.EnsureSuccessStatusCode();
        Assert.IsNotNull(responce);
    }

    [Test]
    public async Task Refresh_InvalidRefreshToken_BadRequestResult()
    {
        //Arrange
        var request = new GetAccesTokenUsingRefreshTokenQuery
        {
            AccessToken = loginResponce.AccessToken,
            RefreshToken = "sdf" //invalid refreshToken
        };

        //Act
        var responce = await _client.PostAsJsonAsync("api/Auth/refresh", request);

        //Assert
        responce.IsSuccessStatusCode.Should().Be(false);
    }

    [Test]
    public async Task Refresh_InvalidAccessToken_BadRequestResult()
    {
        //Arrange
        var request = new GetAccesTokenUsingRefreshTokenQuery
        {
            AccessToken = "sdf", //invalid accessToken
            RefreshToken = loginResponce.RefreshToken
        };

        //Act
        var responce = await _client.PostAsJsonAsync("api/Auth/refresh", request);

        //Assert
        responce.IsSuccessStatusCode.Should().Be(false);
    }

    [Test]
    public async Task Refresh_NonRelatedTokens_BadRequestResult()
    {
        //Arrange
        var registerCommand2 = new RegisterUserCommand
        {
            Email = "test2@example.com",
            Password = "password@#12P2",
            FirstName = "Alex",
            Username = "@Alex222",
            LastName = "Makedonsky",
            PhoneNumber = "123456789"
        };
        var loginCommand2 = new LoginUserCommand
        {
            Username = "@Alex222",
            Password = "password@#12P2"
        };

        await RegisterUser(registerCommand2);
        var loginResponce2 = await LoginUser(loginCommand2);

        var request = new GetAccesTokenUsingRefreshTokenQuery()
        {
            AccessToken = loginResponce.AccessToken,
            RefreshToken = loginResponce2.RefreshToken
        };
        //Act
        var responce = await _client.PostAsJsonAsync("api/Auth/refresh", request);

        //Assert
        responce.IsSuccessStatusCode.Should().Be(false);
    }


    private async Task RegisterUser(RegisterUserCommand command)
    {
        var response = await _client.PostAsJsonAsync("api/Auth/register", command);
        string stringResponce = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
    }
    private async Task<JwtTokenResponse> LoginUser(LoginUserCommand command)
    {
        var response = await _client.PostAsJsonAsync("api/Auth/login", command);

        response.EnsureSuccessStatusCode();
        string stringResponce = await response.Content.ReadAsStringAsync();
        var loginResponce = JsonSerializer.Deserialize<JwtTokenResponse>(stringResponce);
        return loginResponce;
    }
}
