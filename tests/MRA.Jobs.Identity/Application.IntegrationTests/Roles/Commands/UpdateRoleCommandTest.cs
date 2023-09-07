using System.Net.Http.Json;
using MRA.Identity.Application.Contract.ApplicationRoles.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Roles.Commands;
public class UpdateRoleCommandTest : BaseTest
{
    [Test]
    public async Task UpdateRoleCommand_ShouldUpdateRoleCommand_Success()
    {
        var role = new ApplicationRole
        {
            Name = "Test3",
            Slug = "test3"
        };

        await AddEntity(role);

        var command = new UpdateRoleCommand
        {
            NewRoleName = "Test4",
            Slug = role.Slug
        };

        var response = await _client.PutAsJsonAsync($"/api/roles/{role.Slug}", command);


        response.EnsureSuccessStatusCode();
    }
}
