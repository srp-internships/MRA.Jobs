using System.Net;

namespace MRA.Jobs.Application.IntegrationTests.Experiences.Queries;
public class GetUserExperiencesTest : BaseTest
{
    [Test]
    public async Task GetUserExperiences_ShouldReturnUserExperiences_Success()
    {
        await AddApplicantAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile/GetExperiencesByUser");
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task GetUserExperiences_ShouldReturnUserExperiences_AccessIsDenied()
    {
        await AddApplicantAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile/GetExperiencesByUser?userName=amir");

        var message = await response.Content.ReadAsStringAsync();

        Assert.AreEqual("Access is denied", message);
    }

    [Test]
    public async Task GetUserExperiencesByUserName_ShouldReturnUserExperiencesByUserName_Success()
    {
        await AddReviewerAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile/GetExperiencesByUser?userName=@Alex33");

        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task GetUserExperiencesByUserName_ShouldReturnUserExperiencesByUserName_NotFound()
    {
        await AddReviewerAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile/GetExperiencesByUser?userName=@Alex34");

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

}
