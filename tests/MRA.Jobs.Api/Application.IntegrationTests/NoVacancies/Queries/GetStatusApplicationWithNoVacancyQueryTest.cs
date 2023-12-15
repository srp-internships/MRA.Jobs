using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.NoVacancies.Responses;
using MRA.Jobs.Application.IntegrationTests.Application;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.NoVacancies.Queries;

public class GetStatusApplicationWithNoVacancyQueryTest : CreateApplicationTestsBase
{
    [Test]
    public async Task GetStatusApplicationWithHiddenVacancyQuery_1_Return_Unauthorized()
    {
        var response = await _httpClient.GetAsync("api/NoVacancies/GetApplicationStatus");

        Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task GetStatusApplicationWithHiddenVacancyQuery_2_Return_AppliedFalse()
    {
        
        RunAsDefaultUserAsync("applicant3");
        var response = await _httpClient.GetAsync("api/NoVacancies/GetApplicationStatus");
        response.EnsureSuccessStatusCode();
        var applicationStatus = await response.Content.ReadFromJsonAsync<ApplicationWithNoVacancyStatus>();

        Assert.IsTrue(applicationStatus.Applied == false);
    }

    [Test]
    public async Task GetStatusApplicationWithHiddenVacancyQuery_3_Return_ApplicationStatusNotNull()
    {
        var noVacancyResponse = await _httpClient.GetAsync("api/NoVacancies");
        noVacancyResponse.EnsureSuccessStatusCode();
        var noVacancy = await noVacancyResponse.Content.ReadFromJsonAsync<NoVacancyResponse>();

        var createApplicationCommand = new CreateApplicationCommand
        {
            VacancyId = noVacancy.Id, CoverLetter = RandomString(150), Cv = { IsUploadCvMode = false, }
        };
        RunAsDefaultUserAsync("applicant2");
        var response1 = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, createApplicationCommand);
        response1.EnsureSuccessStatusCode();

        var response2 = await _httpClient.GetAsync("api/NoVacancies/GetApplicationStatus");
        response2.EnsureSuccessStatusCode();
        var applicationStatus =
            (await response2.Content.ReadFromJsonAsync<ApplicationWithNoVacancyStatus>()).Status;


        Assert.IsNotNull(applicationStatus);
    }
}