using System.Net;
using System.Net.Http.Json;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Users.Query;

public class GetUserByIdQueryTest : BaseTest
{
    [Test]
    public async Task GetUserByIdQuery_return_Unauthorized()
    {
        
        var user =  new ApplicationUser()
        {
            UserName = "testUser1",
            Email = "testUser1@gmail.com",
            PhoneNumber = "+992987654122",
            FirstName = "Test1",
            LastName = "Test1",
            PhoneNumberConfirmed = true,
            EmailConfirmed = true
        };
        
        await AddEntity(user);

        var response = await _client.GetAsync($"api/User/GetById?userId={user.Id}");
        Assert.That(response.StatusCode is HttpStatusCode.Unauthorized);
    }
    
    [Test]
    public async Task GetUserByIdQuery_returnUserResponse()
    {
        var user = new ApplicationUser()
        {
            UserName = "testUser2",
            Email = "testUser2@gmail.com",
            PhoneNumber = "+992987654123",
            FirstName = "Test2",
            LastName = "Test2",
            PhoneNumberConfirmed = true,
            EmailConfirmed = true
        };

        await AddEntity(user);

        await AddAuthorizationAsync();

        var response = await _client.GetAsync($"api/User/GetById?userId={user.Id}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<UserResponse>();
        Assert.That(result is not null);
    }
}