using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Domain.Entities;
using MRA.Configurations.Common.Constants;

namespace MRA.Jobs.Application.IntegrationTests;

[TestFixture]
public class RegistrationTests : BaseTest
{
    [Test]
    [TestCase("Alex99","alex99@example.com", "+992123451789")]
    [TestCase(" Jason ","jason@example.com", "+992223451789")]
    [TestCase("Piter ","piter@example.com", "+992323451789")]

    public async Task Register_ValidRequestWithCorrectRegisterData_ReturnsOkAndSavesUserIntoDb(string userName, string email, string phoneNumber)
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = email,
            Password = "password@#12P",
            FirstName = "Alex",
            Username = userName,
            LastName = "Makedonsky",
            PhoneNumber = phoneNumber,
            ConfirmPassword = "password@#12P"
        };
        var res = await _client.GetAsync($"api/sms/send_code?PhoneNumber={request.PhoneNumber}");
        res.EnsureSuccessStatusCode();

        request.VerificationCode = (await GetEntity<ConfirmationCode>(x => x.PhoneNumber == request.PhoneNumber)).Code;
        // Assert
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        // Assert
        var registeredUser =
            await GetEntity<ApplicationUser>(u => u.Email == request.Email && u.UserName == request.Username);
        Assert.That(registeredUser,Is.Not.Null, "Registered user not found");
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
            PhoneNumber = "+992523456789",
        };

        // Assert
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Register_InvalidRequestWithEmptyRegisterData_ReturnsBadRequest()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            // Empty Register Data
            Password = "password@1",
            Username = "test_"
        };

        // Assert
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest),
            await response.Content.ReadAsStringAsync());
    }

    [Test]
    [TestCase(" ")]
    [TestCase("$user")]
    [TestCase("User User")]
    [TestCase("user 123")]
    public async Task Register_WrongUsernameRegisterData_ReturnsBadRequest(string userName)
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "test01@easdxa123123mple.com",
            Password = "passdsword@#12P",
            FirstName = "Ale123x",
            Username = userName,
            LastName = "Makesddonsky",
            PhoneNumber = "+992623456701",
            ConfirmPassword = "passdsword@#12P"
        };

        // Assert
        var res = await _client.GetAsync($"api/sms/send_code?PhoneNumber={request.PhoneNumber}");
        res.EnsureSuccessStatusCode();

        request.VerificationCode = (await GetEntity<ConfirmationCode>(x => x.PhoneNumber == request.PhoneNumber)).Code;
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest),
            await response.Content.ReadAsStringAsync());
    }
   

    [Test]
    public async Task Register_WhenCall_CreateRequiredClaims()
    {
        var role = new ApplicationRole
        {
            Id = Guid.NewGuid(), Name = "TestRole2", NormalizedName = "testrole2", Slug = "testrole2-claim"
        };

        await AddEntity(role);

        var request = new RegisterUserCommand
        {
            Email = "test3@rrrexample.com",
            Password = "passworrrrd@#12P",
            FirstName = "Alerrx",
            Username = "@Alerrrx223",
            LastName = "Makerradonsky",
            PhoneNumber = "+992723456789",
            ConfirmPassword = "passworrrrd@#12P"
        };
        var res = await _client.GetAsync($"api/sms/send_code?PhoneNumber={request.PhoneNumber}");
        res.EnsureSuccessStatusCode();

        request.VerificationCode = (await GetEntity<ConfirmationCode>(x => x.PhoneNumber == request.PhoneNumber)).Code;
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        response.IsSuccessStatusCode.Should().BeTrue();

        var user = await GetEntity<ApplicationUser>(s => s.UserName == request.Username);

        var userClaims = await GetWhere<ApplicationUserClaim>(s => s.UserId == user.Id);
        
        Assert.That(userClaims.Exists(s=>s.ClaimType==ClaimTypes.Application && s.ClaimValue=="MraJobs"));
        Assert.That(userClaims.Exists(s=>s.ClaimType==ClaimTypes.Id && s.ClaimValue==user.Id.ToString()));
        Assert.That(userClaims.Exists(s=>s.ClaimType==ClaimTypes.Email && s.ClaimValue==request.Email));
        Assert.That(userClaims.Exists(s=>s.ClaimType==ClaimTypes.Username && s.ClaimValue==request.Username));
    }
}