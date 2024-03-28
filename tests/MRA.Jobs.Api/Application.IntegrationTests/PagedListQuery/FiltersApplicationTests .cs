using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using System.Net.Http.Json;
using MRA.Jobs.Application.IntegrationTests.Application;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using NUnit.Framework;
using MRA.Identity.Application.Contract.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using System.Net;

namespace MRA.Jobs.Application.IntegrationTests.PagedList;
public class Filters_Application_Tests  : CreateApplicationTestsBase
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
    public async Task GetApplicationsWithPagination_WithPagination()
    {
        ResetState();
        await CreateApplications("JobVacancyTest123456");

        RunAsDefaultUserAsync("applicant1");

        var parameters = new Dictionary<string, string>
        {
            { "Page", "1" },
            { "PageSize", "1" }
        };
        var url = "/api/applications?" + string.Join("&", parameters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        var response = await _httpClient.GetAsync(url);
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<PagedList<ApplicationListDto>>();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.PageSize, Is.EqualTo(1));
        Assert.That(result.CurrentPageNumber, Is.EqualTo(1));
        Assert.That(result.Items.Count, Is.EqualTo(1));
        Assert.That(result.Items.All(a => a.ApplicantUsername == "applicant1"));
        
    }
    [Test]
    public async Task GetApplications_WithFilters_ReturnsFilteredApplicantUsername()
    {
        ResetState();
        await CreateApplications("JobVacancyTestA1");

        RunAsDefaultUserAsync("applicant1");

        var filters = new Dictionary<string, string>
            {
                { "Filters", "ApplicantUsername@=applicant1" }
            };

        var url = "/api/applications?" + string.Join("&", filters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        var response = await _httpClient.GetAsync(url);
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<PagedList<ApplicationListDto>>();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Items.All(a => a.ApplicantUsername == "applicant1"));
    }
    [Test]
    public async Task GetApplications_WithFilters_ReturnsFilteredEmail()
    {
        ResetState();
        await CreateApplications("JobVacancyTestA2");

        RunAsDefaultUserAsync("applicant1");

        var filters = new Dictionary<string, string>
            {
                { "Email", "fakeEmail@asd.com" }
            };

        var url = "/api/applications?" + string.Join("&", filters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        var response = await _httpClient.GetAsync(url);
        Assert.That(HttpStatusCode.OK == response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PagedList<ApplicationListDto>>();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Items.All(a => a.ApplicantUsername == "applicant1"));
    }
}


