using System.Net.Http.Json;
using MRA.Identity.Application.Contract.Experiences.Commands.Create;

namespace MRA.Jobs.Application.IntegrationTests.Experiences.Commands;
public class CreateUserExperienceDetailCommandTest : BaseTest
{
    [Test]
    public async Task CreateUserExperienceDetailCommand_ShouldCreateExperienceDetailComman_Success()
    {
        await AddApplicantAuthorizationAsync();

        var command = new CreateExperienceDetailCommand()
        {
            CompanyName="test",
            Description="test",
            JobTitle="test",
            Address="test",
            IsCurrentJob=false,
            StartDate = DateTime.Now.AddYears(-5),
            EndDate = DateTime.Now.AddYears(-1),
        };

        var response = await _client.PostAsJsonAsync("/api/Profile/СreateExperienceDetail", command);
        response.EnsureSuccessStatusCode();
    }
}
