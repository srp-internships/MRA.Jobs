using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.HiddenVacancies.Responses;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.HiddenVacancies.Queries;

public class GetFirstHiddenVacancyQueryTest : Testing
{
    [Test]
    public async Task Return_First_HiddenVacancy()
    {
        var response = await _httpClient.GetAsync("/api/hiddenvacancies");
       
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var vacancy = await response.Content.ReadFromJsonAsync<HiddenVacancyResponse>();
        
        Assert.IsNotNull(vacancy);
    }
}