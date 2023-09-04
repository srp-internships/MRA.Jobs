using System.Net.Http.Json;
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.UserRoles.Commands;
public class CreateUserRolesCommandTest : BaseTest
{
    [Test]
    public async Task CreateUserRole_ShouldCreateUserRole_Success()
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

        var command = new CreateUserRolesCommand
        {
            UserId = user.Id,
            RoleId = role.Id
        };

        var response = await _client.PostAsJsonAsync("/api/userRoles", command);

        response.EnsureSuccessStatusCode();
      
    }


}
