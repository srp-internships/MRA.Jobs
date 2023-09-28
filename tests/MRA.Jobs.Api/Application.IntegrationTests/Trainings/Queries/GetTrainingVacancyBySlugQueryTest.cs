using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
using System.Net;
using NUnit.Framework;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Domain.Entities;
using System.Net.Http.Json;

namespace MRA.Jobs.Application.IntegrationTests.Trainings.Queries;
public class GetTrainingVacancyBySlugQueryTest : Testing
{
    private TrainingsContext _context;

    [SetUp]
    public void SetUp()
    {
        _context = new TrainingsContext();
    }
    [Test]
    public async Task GetTrainingsVacancyBySlugQuery_IfNotFound_ReturnNotFoundTrainingsVacancySlug()
    {
        // Arrange
        var query = new GetTrainingsVacancyBySlugQuery { Slug = "summer-Trainings" };

        // Act
        var response = await _httpClient.GetAsync($"/api/Trainings/{query.Slug}");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Test]
    public async Task GetTrainingsVacancyBySlug_IfFound_ReturnTrainingsVacancy()
    {
        //Arrange 
        var query = new GetInternshipVacancyBySlugQuery { Slug = (await _context.GetTraining("Autumn Trainings")).Slug };

        //Act
        var response = await _httpClient.GetAsync($"/api/Trainings/{query.Slug}");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var trainingsVacancy = await response.Content.ReadFromJsonAsync<TrainingVacancy>();
        Assert.AreEqual(query.Slug, trainingsVacancy.Slug);
    }
}
