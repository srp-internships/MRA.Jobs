using System.Net.Http.Json;

namespace MRA.Jobs.Application.IntegrationTests.Users.Query;
public class CheckUserNameQueryTest : BaseTest
{

    [Test]
    public async Task UserName_IsAvailable()
    {
        var userName = "AlanWalker";
        var response = await _client.GetAsync($"/api/User/CheckUserName/{userName}");

        var isAvailable = !(await response.Content.ReadFromJsonAsync<bool>());

        Assert.IsTrue(isAvailable);
    }

    [Test]
    public async Task UserName_IsNotAvailable()
    {
        var userName = "@Alex33";
        var response = await _client.GetAsync($"/api/User/CheckUserName/{userName}");

        var isAvailable = !(await response.Content.ReadFromJsonAsync<bool>());

        Assert.IsFalse(isAvailable);
    }

}
