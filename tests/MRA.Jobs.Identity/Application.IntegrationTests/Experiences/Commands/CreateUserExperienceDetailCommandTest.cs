using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.Experiences.Commands.Create;

namespace MRA.Jobs.Application.IntegrationTests.Experiences.Commands;
public class CreateUserExperienceDetailCommandTest : BaseTest
{
    [Test]
    public async Task CreateUserExperienceDetailCommand_ShouldCreateExperienceDetailCommand_Success()
    {
        await AddApplicantAuthorizationAsync();

        var command = new CreateExperienceDetailCommand()
        {
            CompanyName = "test",
            Description = "test",
            JobTitle = "test",
            Address = "test",
            IsCurrentJob = false,
            StartDate = DateTime.Now.AddYears(-5),
            EndDate = DateTime.Now.AddYears(-1),
        };

        var response = await _client.PostAsJsonAsync("/api/Profile/CreateExperienceDetail", command);
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task CreateUserExperienceDetailCommand_WhenRequestWillCreateDuplicateExperience_ReturnsDuplicate()
    {
        await AddApplicantAuthorizationAsync();

        var command = new CreateExperienceDetailCommand()
        {
            CompanyName = "SRP",
            Description = "test",
            JobTitle = "Backend Developer",
            Address = "test",
            IsCurrentJob = false,
            StartDate = DateTime.Now.AddYears(-5),
            EndDate = DateTime.Now.AddYears(-1),
        };

        var response = await _client.PostAsJsonAsync("/api/Profile/CreateExperienceDetail", command);
        response.EnsureSuccessStatusCode();

        command.CompanyName = "srp";
        command.JobTitle = "backend developer";

        response = await _client.PostAsJsonAsync("/api/Profile/CreateExperienceDetail", command);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsTrue((await response.Content.ReadFromJsonAsync<ProblemDetails>()).Detail.Contains("Experience detail already exists"));

    }
}
