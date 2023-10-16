using System.Net.Http.Json;
using MRA.Identity.Application.Contract.Skills.Command;

namespace MRA.Jobs.Application.IntegrationTests.Skills.Commands;
public class AddUserSkillsCommandTest : BaseTest
{
    [Test]
    public async Task AddUserSkills_ShouldAddUserSkills_Success()
    {
        var command = new AddSkillsCommand()
        {
            Skills = new List<string>() { "Skill1", "Skill2" }
        };

        await AddApplicantAuthorizationAsync();

        var response =await _client.PostAsJsonAsync("/api/Profile/AddSkills", command);

        response.EnsureSuccessStatusCode();
    }


}
