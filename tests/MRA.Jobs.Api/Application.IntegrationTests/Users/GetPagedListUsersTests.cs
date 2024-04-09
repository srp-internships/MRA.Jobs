using System.Net;
using System.Net.Http.Json;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Common;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Users;

public class GetPagedListUsersTests : Testing
{
    [Test]
    public async Task GetPagedListUsers_Return_PagedListUserResponse()
    {
        RunAsReviewerAsync();
        var response = await _httpClient.GetFromJsonAsync<PagedList<UserResponse>>("api/user");
        Assert.That(response.Items.Count == 15);
    }

    [Test]
    public async Task GetPagedListUsers_Return_Unauthorized()
    {
        var response = await _httpClient.GetAsync("api/user");
        
        Assert.That(response.StatusCode==HttpStatusCode.Unauthorized);
    }
    
}