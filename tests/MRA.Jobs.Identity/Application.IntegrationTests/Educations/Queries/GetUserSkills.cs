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
    public async Task GetUserSkill_ShouldReturnUserEducations_AccessIsDenied()
    {
        await AddApplicantAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile/GetEducationsByUser?userName=amir");

        var message = await response.Content.ReadAsStringAsync();

        Assert.AreEqual("Access is denied", message);
    }

    [Test]
    public async Task GetUserSkillByUserName_ShouldReturnUserEducationsByUserName_Success()
    {
        await AddReviewerAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile/GetEducationsByUser?userName=@Alex33");

        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task GetUserSkillByUserName_ShouldReturnUserEducationsByUserName_NotFound()
    {
        await AddReviewerAuthorizationAsync();
        var response = await _client.GetAsync($"/api/Profile/GetEducationsByUser?userName=@Alex34");

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

}
