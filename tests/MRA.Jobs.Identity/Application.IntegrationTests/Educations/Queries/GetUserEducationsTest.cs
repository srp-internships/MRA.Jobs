using System.Net;

namespace MRA.Jobs.Application.IntegrationTests.Educations.Queries;

public class GetUserEducationsTest : BaseTest
{
    [Test]
    public async Task GetUserEducations_ShouldReturnUserEducations_Success()
    {
        await AddApplicantAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile/GetEducationsByUser");
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task GetUserEducations_ShouldReturnUserEducations_AccessIsDenied()
    {
        await AddApplicantAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile/GetEducationsByUser?userName=amir");

        Assert.That(HttpStatusCode.Forbidden == response.StatusCode);
    }

    [Test]
    public async Task GetUserEducationsByUserName_ShouldReturnUserEducationsByUserName_Success()
    {
        await AddReviewerAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile/GetEducationsByUser?userName=@Alex33");

        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task GetUserEducationsByUserName_ShouldReturnUserEducationsByUserName_NotFound()
    {
        await AddReviewerAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile/GetEducationsByUser?userName=@Alex34");

        Assert.That(HttpStatusCode.NotFound == response.StatusCode);
    }
}