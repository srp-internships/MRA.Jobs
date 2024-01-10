using System.Net;

namespace MRA.Jobs.Application.IntegrationTests.Skills.Queries;

public class GetUserSkills
{
    public class GetUserSkillsTest : BaseTest
    {
        [Test]
        public async Task GetUserSkills_ShouldReturnUserSkills_Success()
        {
            await AddApplicantAuthorizationAsync();
            var response = await _client.GetAsync($"/api/Profile/GetUserSkills");
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetUserSkill_ShouldReturnUserSkills_AccessIsDenied()
        {
            await AddApplicantAuthorizationAsync();
            var response = await _client.GetAsync($"/api/Profile/GetUserSkills?userName=amir");

            Assert.That(HttpStatusCode.Forbidden == response.StatusCode);
        }

        [Test]
        public async Task GetUserSkillByUserName_ShouldReturnUserSkillsByUserName_Success()
        {
            await AddReviewerAuthorizationAsync();
            var response = await _client.GetAsync($"/api/Profile/GetUserSkills?userName=@Alex33");

            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetUserSkillByUserName_ShouldReturnUserSkillsByUserName_NotFound()
        {
            await AddReviewerAuthorizationAsync();
            var response = await _client.GetAsync($"/api/Profile/GetUserSkills?userName=@Alex34");

            Assert.That(HttpStatusCode.NotFound == response.StatusCode);
        }
    }
}