using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MRA.Identity.Application.Contract.Skills.Command;
using MRA.Identity.Domain.Entities;
using System.Net.Http.Json;

namespace MRA.Jobs.Application.IntegrationTests.Skills.Commands;
public class RemoveUserSkillCommandTest : BaseTest
{
    [Test]
    public async Task RemoveUserSkill_ShouldRemoveUserSkill_Success()
    {
        var skill = new UserSkill()
        {
            Skill = new Skill { Name = "Sklil3" },
            UserId = ApplicationId
        };
        await AddEntity(skill);

        var command = new RemoveUserSkillCommand()
        {
            Skill = "Sklil3"
        };

        await AddApplicantAuthorizationAsync();

        var response = await _client.DeleteAsync($"/api/Profile/RemoveUserSkill/{command.Skill}");

        response.EnsureSuccessStatusCode();
    }
}
