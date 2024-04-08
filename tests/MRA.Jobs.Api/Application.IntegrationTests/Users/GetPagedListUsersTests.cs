using System.Net.Http.Json;
using System.Web;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Users;
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
    public async Task GetPagedListUsersByFilter_ReturnPagedListUserResponse()
    {
        var query = new GetPagedListUsersQuery() { Filters = "FullName@=Full Name 2" };
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString["Filters"] = query.Filters;
        RunAsReviewerAsync();
        var response = await _httpClient.GetFromJsonAsync<PagedList<UserResponse>>($"api/user?{queryString}");
        Assert.That(response.Items.Count == 1);
        Assert.That(response.Items[0].UserName == "User2");
    }

}