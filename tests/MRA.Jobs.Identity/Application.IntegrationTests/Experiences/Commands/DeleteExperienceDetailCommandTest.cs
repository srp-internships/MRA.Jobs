using MRA.Identity.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Experiences.Commands;
public class DeleteExperienceDetailCommandTest : BaseTest
{
    [Test]
    public async Task DeleteExperienceDetailCommand_ShouldDeleteExperienceDetiel_Success()
    {
        await AddApplicantAuthorizationAsync();

        var experience = new ExperienceDetail()
        {
            CompanyName = "test2",
            Description = "test2",
            JobTitle = "test2",
            Address = "test2",
            IsCurrentJob = false,
            StartDate = DateTime.Now.AddYears(-5),
            EndDate = DateTime.Now.AddYears(-1),
            UserId = Applicant.Id,
        };
        await AddEntity(experience);

        var response = await _client.DeleteAsync($"/api/Profile/DeleteExperienceDetail/{experience.Id}");

        response.EnsureSuccessStatusCode();
    }
}
