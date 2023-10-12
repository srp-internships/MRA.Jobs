using System.Net.Http.Json;
using MRA.Identity.Application.Contract.Experiences.Commands.Update;
using MRA.Identity.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Experiences.Commands;
public class UpdateUserExperienceDetailCommandTest : BaseTest
{
    [Test]
    public async Task UpdateUserEductionDetailCommand_ShouldUpdateUserEductionDetailCommand_Success()
    {
        await AddApplicantAuthorizationAsync();

        var experience = new ExperienceDetail()
        {
            CompanyName = "test3",
            Description = "test3",
            JobTitle = "test3",
            Address = "test3",
            IsCurrentJob = false,
            StartDate = DateTime.Now.AddYears(-5),
            EndDate = DateTime.Now.AddYears(-1),
            UserId = Applicant.Id,
        };

        await AddEntity(experience);

        var command = new UpdateExperienceDetailCommand()
        {
            Id = experience.Id,
            CompanyName = "test4",
            Description = "test4",
            JobTitle = "test4",
            Address = "test4",
            IsCurrentJob = false,
            StartDate = DateTime.Now.AddYears(-5),
            EndDate = DateTime.Now.AddYears(-1),
        };

        var response2 = await _client.PutAsJsonAsync("/api/Profile/UpdateExperienceDetail", command);

        response2.EnsureSuccessStatusCode();
    }
}
