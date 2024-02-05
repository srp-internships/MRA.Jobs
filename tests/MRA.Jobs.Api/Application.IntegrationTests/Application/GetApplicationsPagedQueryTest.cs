using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Application;

public class GetApplicationsPagedQueryTest : CreateApplicationTestsBase
{
    private async Task CreateApplications(string jobTitle)
    {
        var vacancySlug1 = await AddJobVacancyAsync(jobTitle);
        var testSubmit1 = new CreateApplicationCommand
        {
            VacancySlug = vacancySlug1,
            CoverLetter = RandomString(150),
            Cv = { IsUploadCvMode = true, CvBytes = new byte[] { 1, 2, 3 }, FileName = "213.bytes" }
        };
        RunAsDefaultUserAsync("applicant1");
        await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, testSubmit1);


        var vacancySlug2 = await AddJobVacancyAsync(jobTitle);
        var testSubmit2 = new CreateApplicationCommand
        {
            VacancySlug = vacancySlug2,
            CoverLetter = RandomString(150),
            Cv = { IsUploadCvMode = true, CvBytes = new byte[] { 1, 2, 3 }, FileName = "213.bytes" }
        };
        RunAsDefaultUserAsync("applicant2");
        await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, testSubmit2);
    }

    [Test]
    public async Task GetApplications_OnlyApplicantRole_ReturnsFilteredApplications()
    {
        ResetState();
        await CreateApplications("JobVacancyTest1");

        RunAsDefaultUserAsync("applicant1");

        var response = await _httpClient.GetAsync(ApplicationApiEndPoint);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PagedList<ApplicationListDto>>();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Items, Is.Not.Empty);
        Assert.That(result.Items.All(a => a.Username == "applicant1"));
        Assert.That(result.Items.Count == 1);
    }

    [Test]
    public async Task GetApplications_OnlyReviewerRole_ReturnsAllApplications()
    {
        await CreateApplications("JobVacancyTest1");

        RunAsReviewerAsync();

        var response = await _httpClient.GetAsync(ApplicationApiEndPoint);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PagedList<ApplicationListDto>>();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Items, Is.Not.Empty);
        Assert.That(result.Items.Any(a => a.Username == "applicant1"));
        Assert.That(result.Items.Any(a => a.Username == "applicant2"));
    }
}