using System.Net.Http.Json;
using MRA.Identity.Application.Contract.Educations.Command.Create;

namespace MRA.Jobs.Application.IntegrationTests.Educations.Commands;
public class CreateUserEducationDetailCommandTest : BaseTest
{
    [Test]
    public async Task CreateUserEducationDetailCommand_ShouldCreateEducationDetailComman_Success()
    {
        await AddApplicantAuthorizationAsync();

        var command = new CreateEducationDetailCommand()
        {
            Speciality = "test",
            University = "test",
            StartDate = DateTime.Now.AddYears(-5),
            EndDate = DateTime.Now.AddYears(-1),
            UntilNow = false,
        };

        var response = await _client.PostAsJsonAsync("/api/Profile/CreateEducationDetail", command);
        response.EnsureSuccessStatusCode();
    }
}
