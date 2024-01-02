using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobVacancyBySlug;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Jobs.Queries;

public class GetJobVacancyBySlugQueryTest : Testing
{
    private JobsContext _context;

    [SetUp]
    public void SetUp()
    {
        _context = new JobsContext();
    }

    [Test]
    public async Task GetJobVacancyBySlugQuery_IfNotFound_ReturnNotFoundJobVacancySlug()
    {
        // Arrange
        var query = new GetJobVacancyBySlugQuery { Slug = "c-sharp-developer" };

        // Act
        var response = await _httpClient.GetAsync($"/api/jobs/{query.Slug}");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Test]
    public async Task GetJobVacancyBySlug_IfFound_ReturnJobVacancy()
    {
        //Arrange 
        var query = new GetJobVacancyBySlugQuery
        {
            Slug = (
                await _context.GetJob("Backend Developer", DateTime.Now.AddDays(2))).Slug
        };

        //Act
        var response = await _httpClient.GetAsync($"/api/jobs/{query.Slug}");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var jobVacancy = await response.Content.ReadFromJsonAsync<JobVacancy>();
        Assert.AreEqual(query.Slug, jobVacancy.Slug);
    }
}