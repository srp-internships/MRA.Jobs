using System.Net;

namespace MRA.Jobs.Application.IntegrationTests.UserProfile;

public class GetProfileUserQueryTest : BaseTest
{
    [Test]
    public async Task GetProfile_ShouldReturnProfile_Success()
    {
        await AddApplicantAuthorizationAsync();
        var response = await _client.GetAsync("/api/Profile");
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task GetProfileQuery_ShouldReturnProfile_AccessIsDenied()
    {
        await AddApplicantAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile?userName=amir");


        Assert.That(HttpStatusCode.Forbidden == response.StatusCode);
    }

    [Test]
    public async Task GetProfileByUserName_ShouldReturnProfileByUserName_Success()
    {
        await AddReviewerAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile?userName=@Alex33");

        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task GetProfileByUserName_ShouldReturnProfileByUserName_NotFound()
    {
        await AddReviewerAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile?userName=@Alex34");

        Assert.That(HttpStatusCode.NotFound == response.StatusCode);
    }
}