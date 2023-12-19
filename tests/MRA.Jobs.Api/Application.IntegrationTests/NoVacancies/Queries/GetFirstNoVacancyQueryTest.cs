using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.NoVacancies.Responses;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.NoVacancies.Queries;

public class GetFirstNoVacancyQueryTest : Testing
{
    [Test]
    public async Task Return_First_NoVacancy()
    {
        var response = await _httpClient.GetAsync("/api/NoVacancies");
       
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var vacancy = await response.Content.ReadFromJsonAsync<NoVacancyResponse>();
        
        Assert.IsNotNull(vacancy);
    }
}