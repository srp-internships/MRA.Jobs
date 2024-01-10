using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.Educations.Command.Create;

namespace MRA.Jobs.Application.IntegrationTests.Educations.Commands;

public class CreateUserEducationDetailCommandTest : BaseTest
{
    [Test]
    public async Task CreateUserEducationDetailCommand_ShouldCreateEducationDetailCommand_Success()
    {
        await AddApplicantAuthorizationAsync();

        var command = new CreateEducationDetailCommand()
        {
            Speciality = "test test",
            University = "test",
            StartDate = DateTime.Now.AddYears(-5),
            EndDate = DateTime.Now.AddYears(-1),
            UntilNow = false,
        };

        var response = await _client.PostAsJsonAsync("/api/Profile/CreateEducationDetail", command);
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task CreateUserEducationDetailCommand_DuplicateRequest_ReturnsConflict()
    {
        await AddApplicantAuthorizationAsync();

        var command = new CreateEducationDetailCommand()
        {
            Speciality = "IT Security",
            University = "Khujand University",
            StartDate = DateTime.Now.AddYears(-5),
            EndDate = DateTime.Now.AddYears(-1),
            UntilNow = false,
        };


        var response = await _client.PostAsJsonAsync("/api/Profile/CreateEducationDetail", command);
        Assert.That(response.StatusCode == HttpStatusCode.OK);

        command.Speciality = "It security";
        command.University = "khujand university";
        response = await _client.PostAsJsonAsync("/api/Profile/CreateEducationDetail", command);
        Assert.That(HttpStatusCode.BadRequest == response.StatusCode);
        Assert.That(
            (await response.Content.ReadFromJsonAsync<ProblemDetails>()).Detail!.Contains(
                "Education detail already exists"));
    }
}