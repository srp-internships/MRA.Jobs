using System.Net;
using System.Net.Http.Json;
using Azure;
using Microsoft.AspNetCore.Mvc;
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

    [Test]
    public async Task UpdateUserEductionDetailCommand_ShouldUpdateUserEductionDetailCommand_Duplicate()
    {
        await AddApplicantAuthorizationAsync();

        var experience = new ExperienceDetail()
        {
            CompanyName = "test5",
            Description = "test5",
            JobTitle = "test5",
            Address = "test5",
            IsCurrentJob = false,
            StartDate = DateTime.Now.AddYears(-5),
            EndDate = DateTime.Now.AddYears(-1),
            UserId = Applicant.Id,
        };
        var experience2 = new ExperienceDetail()
        {
            CompanyName = "test6",
            Description = "test6",
            JobTitle = "test6",
            Address = "test6",
            IsCurrentJob = false,
            StartDate = DateTime.Now.AddYears(-5),
            EndDate = DateTime.Now.AddYears(-1),
            UserId = Applicant.Id,
        };

        await AddEntity(experience);
        await AddEntity(experience2);

        var command = new UpdateExperienceDetailCommand()
        {
            Id = experience.Id,
            CompanyName = experience2.CompanyName,
            Description = experience.Description,
            JobTitle = experience2.JobTitle,
            Address = experience.Address,
            IsCurrentJob = false,
            StartDate = DateTime.Now.AddYears(-3),
            EndDate = DateTime.Now.AddYears(-1),
        };

        var response = await _client.PutAsJsonAsync("/api/Profile/UpdateExperienceDetail", command);

        Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        Assert.IsTrue((await response.Content.ReadFromJsonAsync<ProblemDetails>()).Detail.Contains("Experience detail already exists"));
    }
}
