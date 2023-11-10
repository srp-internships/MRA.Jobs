using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Domain.Entities;
using MRA.Configurations.Common.Constants;

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
            FirstName = "Alex",
            Username = "@Alex223",
            LastName = "Makedonsky",
            PhoneNumber = "+992123451789",
            Role = "asdfffssdesasfasefa",
            Application = "43wtruigjklf"
        };

        // Assert
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        // Assert
        var registeredUser =
            await GetEntity<ApplicationUser>(u => u.Email == request.Email && u.UserName == request.Username);
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
        };

        // Assert
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest),
            await response.Content.ReadAsStringAsync());
    }

    [Test]
    public async Task Register_WhenRoleDoesNotExist_CreateRole()
    {
        var request = new RegisterUserCommand
        {
            Email = "test3@easdxa123123mple.com",
            Password = "passdsword@#12P",
            FirstName = "Ale123x",
            Username = "@Alsdex223123",
            LastName = "Makesddonsky",
            PhoneNumber = "+992623456789",
            Role = "Role1",
            Application = "mra.Test"
        };

        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        response.IsSuccessStatusCode.Should().BeTrue();

        var role = await GetEntity<ApplicationRole>(s => s.Name == request.Role);

        var userRole = await GetEntity<ApplicationUserRole>(a => a.RoleId == role.Id);

        Assert.That(role, Is.Not.Null);
        Assert.That(userRole, Is.Not.Null);
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
            Role = role.Name,
            Application = "mra.Test"
        };

        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        response.IsSuccessStatusCode.Should().BeTrue();

        var user = await GetEntity<ApplicationUser>(s => s.UserName == request.Username);

        var userClaims = await GetWhere<ApplicationUserClaim>(s => s.UserId == user.Id);
        
        Assert.That(userClaims.Exists(s=>s.ClaimType==ClaimTypes.Application && s.ClaimValue==request.Application));
        Assert.That(userClaims.Exists(s=>s.ClaimType==ClaimTypes.Id && s.ClaimValue==user.Id.ToString()));
        Assert.That(userClaims.Exists(s=>s.ClaimType==ClaimTypes.Role && s.ClaimValue==request.Role));
        Assert.That(userClaims.Exists(s=>s.ClaimType==ClaimTypes.Email && s.ClaimValue==request.Email));
        Assert.That(userClaims.Exists(s=>s.ClaimType==ClaimTypes.Username && s.ClaimValue==request.Username));
    }
}