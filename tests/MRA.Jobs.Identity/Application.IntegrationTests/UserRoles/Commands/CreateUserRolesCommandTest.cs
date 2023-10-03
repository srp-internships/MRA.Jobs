using System.Net.Http.Json;
using FluentAssertions;
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Domain.Entities;
using Mra.Shared.Common.Constants;

namespace MRA.Jobs.Application.IntegrationTests.UserRoles.Commands;

public class CreateUserRolesCommandTest : BaseTest
{
    [Test]
    public async Task CreateUserRole_ShouldCreateUserRole_Success()
    {
        var user = new ApplicationUser { UserName = "Test", Email = "Test@con.ty", };

        await AddEntity(user);

        var role = new ApplicationRole { Slug = "rol1", Name = "Rol1" };

        await AddEntity(role);

        var command = new CreateUserRolesCommand { UserId = user.Id, RoleName = role.Name };

        await AddAuthorizationAsync();
        var response = await _client.PostAsJsonAsync("/api/userRoles", command);

        response.EnsureSuccessStatusCode();
    }


    [Test]
    public async Task CreateUserRole_ShouldCreateUserRole_ClaimRoleShouldBeCreated()
    {
        var user = new ApplicationUser { UserName = "Test", Email = "Test@con.ty", };

        await AddEntity(user);

        var role = new ApplicationRole { Slug = "rol1", Name = "Rol1" };

        await AddEntity(role);

        var command = new CreateUserRolesCommand { UserId = user.Id, RoleName = role.Name };

        await AddAuthorizationAsync();
        var response = await _client.PostAsJsonAsync("/api/userRoles", command);

        response.EnsureSuccessStatusCode();

        var claim = await GetEntity<ApplicationUserClaim>(s =>
            s.UserId == user.Id && s.ClaimType == ClaimTypes.Role);
        claim.Should().NotBeNull();
    }
}