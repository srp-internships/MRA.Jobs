using System.Net;
using MRA.Identity.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.UserRoles.Commands;
public class DeleteUserRolesCommandTest : BaseTest
{
    [Test]
    public async Task DeleteUserRoleCommand_ShouldDeleteUserRoleCommand_Success()
    {
        var user = new ApplicationUser
        {
            UserName = "Test",
            Email = "Test@con.ty",
        };

        await AddEntity(user);

        var role = new ApplicationRole
        {
            Slug = "rol1",
            Name = "Rol1"
        };

        await AddEntity(role);

        var slug = $"{user.UserName.ToLower()}-{role.Slug}";

        var userRole = new ApplicationUserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            Slug = slug
        };
        await AddEntity(userRole);

        await AddAuthorizationAsync();
        var response = await _client.DeleteAsync($"/api/UserRoles/{slug}");

        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task DeleteUserRoleCommand_ShouldDeleteUserRoleCommand_NotFound()
    {

        await AddAuthorizationAsync();
        var response = await _client.DeleteAsync("/api/UserRoles/testt10");

        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
    }
}
