using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using System.Net.Http.Json;
using MRA.Jobs.Application.IntegrationTests.Application;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using NUnit.Framework;
using MRA.Identity.Application.Contract.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using System.Net;
using Moq;
using Newtonsoft.Json;
using System.Text;
using MRA.Identity.Application.Contract.User.Responses;
using Moq.Protected;

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
    public async Task GetApplications_WithFilters_ReturnsFilteredVacancyTitle()
    {
        ResetState();
        await CreateApplications("JobVacancyTestA2");

        RunAsDefaultUserAsync("applicant1");

        var filters = new Dictionary<string, string>
            {
                { "Filters", "Vacancy.Title@=JobVacancyTestA2" }
            };

        var url = "/api/applications?" + string.Join("&", filters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        var response = await _httpClient.GetAsync(url);
        Assert.That(HttpStatusCode.OK == response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PagedList<ApplicationListDto>>();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Items.All(a => a.VacancyTitle == "JobVacancyTestA2"));
    }
    private HttpClient CreateMockHttpClient(PagedList<ApplicationListDto> mockPagedList)
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(mockPagedList), Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        httpClient.BaseAddress = new Uri("http://localhost7071");
        return httpClient;
    }

    [Test]
    public async Task GetApplications_WithFilters_ReturnsFilteredUser()
    {
        var mockUser = new UserResponse
        {
            UserName = "Bob",
            Email = "email@gmail.com",
            PhoneNumber = "+9999999999",
            FullName = "Ali Aliev Alivich"
        };
        var mockApplicationListDto = new ApplicationListDto
        {
            User = mockUser
        };

        var mockPagedList = new PagedList<ApplicationListDto>
        {
            Items = new List<ApplicationListDto> { mockApplicationListDto }
        };

        _httpClient = CreateMockHttpClient(mockPagedList);

        var url = "/api/applications?PhoneNumber=+9999999999";
        var response = await _httpClient.GetAsync(url);
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<PagedList<ApplicationListDto>>();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Items.All(a => a.User.PhoneNumber == "+9999999999"));
    }

}


