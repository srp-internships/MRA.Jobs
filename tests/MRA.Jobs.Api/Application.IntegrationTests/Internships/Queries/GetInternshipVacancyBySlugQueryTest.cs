using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Internships.Queries;
public class GetInternshipVacancyBySlugQueryTest : Testing
{

    private InternshipsContext _context;

    [SetUp]
    public void SetUp()
    {
        _context = new InternshipsContext();
    }

    [Test]
    public async Task GetInternshipVacancyBySlugQuery_IfNotFound_ReturnNotFoundInternshipVacancySlug()
    {
        // Arrange
        var query = new GetInternshipVacancyBySlugQuery { Slug = "winter-internships" };

        // Act
        var response = await _httpClient.GetAsync($"/api/internships/{query.Slug}");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Test]
    public async Task GetInternshipVacancyBySlug_IfFound_ReturnInternshipVacancy()
    {
        //Arrange 
        var query = new GetInternshipVacancyBySlugQuery { Slug = (await _context.GetInternship("Autumn Internships")).Slug };

        //Act
        var response = await _httpClient.GetAsync($"/api/internships/{query.Slug}");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var InternshipVacancy = await response.Content.ReadFromJsonAsync<InternshipVacancy>();
        Assert.AreEqual(query.Slug, InternshipVacancy.Slug);
    }

}
