using MRA.Identity.Application.Contract.Skills.Queries;

namespace MRA.Jobs.Application.IntegrationTests.Skills.Queries;
public class GetUserSkills
{
    public class GetUserSkillsTest : BaseTest
    {
        [Test]
        public async Task GetUserSkills_ShouldReturnUserSkills_Success()
        {
           
            var request = new GetUserSkillsQuery
            {
                UserName = "@Alex33"
            };

          
            await AddApplicantAuthorizationAsync();

            
            var response = await _client.GetAsync($"/api/Profile/GetUserSkills/{request.UserName}");

            
            response.EnsureSuccessStatusCode();
        }
    }

}
