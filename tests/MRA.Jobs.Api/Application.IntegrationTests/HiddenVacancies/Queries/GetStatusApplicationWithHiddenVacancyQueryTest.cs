using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.HiddenVacancies.Responses;
using MRA.Jobs.Application.IntegrationTests.Application;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.HiddenVacancies.Queries;

public class GetStatusApplicationWithHiddenVacancyQueryTest : CreateApplicationTestsBase
{
    [Test]
    public async Task GetStatusApplicationWithHiddenVacancyQuery_1_Return_Unauthorized()
    {
        var response = await _httpClient.GetAsync("api/HiddenVacancies/GetApplicationStatus");

        Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task GetStatusApplicationWithHiddenVacancyQuery_2_Return_AppliedFalse()
    {
        
        RunAsDefaultUserAsync("applicant3");
        var response = await _httpClient.GetAsync("api/HiddenVacancies/GetApplicationStatus");
        response.EnsureSuccessStatusCode();
        var applicationStatus = await response.Content.ReadFromJsonAsync<ApplicationWithHiddenVacancyStatus>();

        Assert.IsTrue(applicationStatus.Applied == false);
    }

    [Test]
    public async Task GetStatusApplicationWithHiddenVacancyQuery_3_Return_ApplicationStatusNotNull()
    {
        var hiddenVacancyResponse = await _httpClient.GetAsync("api/HiddenVacancies");
        hiddenVacancyResponse.EnsureSuccessStatusCode();
        var hiddenVacancy = await hiddenVacancyResponse.Content.ReadFromJsonAsync<HiddenVacancyResponse>();

        var createApplicationCommand = new CreateApplicationCommand
        {
            VacancyId = hiddenVacancy.Id, CoverLetter = RandomString(150), Cv = { IsUploadCvMode = false, }
        };
        RunAsDefaultUserAsync("applicant2");
        var response1 = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, createApplicationCommand);
        response1.EnsureSuccessStatusCode();

        var response2 = await _httpClient.GetAsync("api/HiddenVacancies/GetApplicationStatus");
        response2.EnsureSuccessStatusCode();
        var applicationStatus =
            (await response2.Content.ReadFromJsonAsync<ApplicationWithHiddenVacancyStatus>()).Status;


        Assert.IsNotNull(applicationStatus);
    }
}