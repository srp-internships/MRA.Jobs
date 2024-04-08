using System.Net;
using System.Net.Http.Json;
using MRA.Identity.Application.Contract.User.Responses;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Users;

public class GetListUsersTests : Testing
{
    [Test]
    public async Task GetPagedListUsers_Return_PagedListUserResponse()
    {
        RunAsReviewerAsync();
        var response = await _httpClient.GetFromJsonAsync<List<UserResponse>>("api/user/List");
        Assert.That(response.Count == 15);
    }

    [Test]
    public async Task GetPagedListUsers_Return_Unauthorized()
    {
        var response = await _httpClient.GetAsync("api/user/List");

        Assert.That(response.StatusCode == HttpStatusCode.Unauthorized);
    }
}