using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Domain.Enums;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Application.Update;

public class ChangeApplicationStatusTests : Testing
{
    [Test]
    [TestCase(ApplicationStatus.Approved)]
    [TestCase(ApplicationStatus.Hired)]
    [TestCase(ApplicationStatus.Rejected)]
    [TestCase(ApplicationStatus.Interviewed)]
    [TestCase(ApplicationStatus.WishList)]
    public async Task ChangeApplicationStatus(ApplicationStatus status)
    {
        ResetState();
        var category = new VacancyCategory() { Name = "Category 123" };
        var vacancy = new JobVacancy()
        {
            Title = "test",
            ShortDescription = "Short Description",
            Description = "ShortDescription",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30),
            Category = category,
            WorkSchedule = WorkSchedule.FullTime,
            Slug = "test-01-01",
            RequiredYearOfExperience = 10
        };

        await AddAsync(vacancy);

        var applicantId = Guid.NewGuid();
        const string userName = "applicant";
        var applicationSlug = "applicant-test-01-01";
        var application = new MRA.Jobs.Domain.Entities.Application()
        {
            Slug = applicationSlug,
            ApplicantId = applicantId,
            VacancyId = vacancy.Id,
            CV = "cv.pdf",
            CreatedBy = applicantId,
            Status = ApplicationStatus.Submitted,
            CoverLetter =
                "I am writing to apply for the [Job Title] position at [Company's Name], as advertised. " +
                "I am drawn to this opportunity for several reasons.",
            ApplicantUsername = userName
        };

        await AddAsync(application);

        RunAsReviewerAsync();

        var command = new UpdateApplicationStatusCommand
        {
            Slug = applicationSlug, StatusId = (int)status, ApplicantUserName = userName
        };

        var response = await _httpClient.PutAsJsonAsync($"/api/applications/{applicationSlug}/update-status",
            command);

        response.EnsureSuccessStatusCode();
        var result = await FindFirstOrDefaultAsync<MRA.Jobs.Domain.Entities.Application>(a => a.Id == application.Id);
        Assert.That(result.Status == status);
    }
}