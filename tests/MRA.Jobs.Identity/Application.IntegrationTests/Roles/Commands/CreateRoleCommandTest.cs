using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MRA.Identity.Application.Contract.ApplicationRoles.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Roles.Commands;
public class CreateRoleCommandTest : BaseTest
{
    [Test]
    public async Task CreateRoleCommand_ShouldCreateRoleCommand_Success()
    {
        var command = new CreateRoleCommand
        {
            RoleName = "Test",
        };

        await AddAuthorizationAsync();
        var response = await _client.PostAsJsonAsync("/api/roles", command);

        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task CreateRoleCommand_ShouldCreateRoleCommand_Failed()
    {
        var role = new ApplicationRole
        {
            Name = "test2",
            Slug = "test2",
            NormalizedName = "TEST2"
        };

        await AddEntity(role);


        var command = new CreateRoleCommand
        {
            RoleName = "test2",
        };
        
        await AddAuthorizationAsync();
        var response = await _client.PostAsJsonAsync("/api/roles", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
