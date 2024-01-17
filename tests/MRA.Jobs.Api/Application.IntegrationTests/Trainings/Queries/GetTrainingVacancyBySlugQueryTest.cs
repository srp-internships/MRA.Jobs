using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
using System.Net;
using NUnit.Framework;
using MRA.Jobs.Domain.Entities;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobVacancyBySlug;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

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
        RunAsReviewerAsync();
        var response = await _httpClient.GetAsync($"/api/Trainings/{query.Slug}");

        // Assert
        Assert.That(HttpStatusCode.NotFound == response.StatusCode);
    }

    [Test]
    public async Task GetTrainingsVacancyBySlug_IfFound_ReturnTrainingsVacancy()
    {
        //Arrange 
        var query = new GetInternshipVacancyBySlugQuery
        {
            Slug = (
                await _context.GetTraining("Autumn Trainings", DateTime.Now.AddDays(2))).Slug
        };

        //Act
        RunAsReviewerAsync();
        var response = await _httpClient.GetAsync($"/api/Trainings/{query.Slug}");

        //Assert
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var trainingsVacancy = await response.Content.ReadFromJsonAsync<TrainingVacancy>();
        Assert.That(query.Slug == trainingsVacancy.Slug);
    }

    [Test]
    public async Task IsApplied_ShouldBeTrue_WhenUserApplied()
    {
        RunAsDefaultUserAsync("testuser");
        //Arrange 
        var vacancy = await AddJobVacancyAsync("Cool training");

        Domain.Entities.Application application = new()
        {
            ApplicantId = GetCurrentUserId(),
            ApplicantUsername = "testuser",
            VacancyId = vacancy.Id,
            Slug = "testuser-cool-training"
        };

        await AddAsync(application);

        var query = new GetTrainingsVacancyBySlugQuery
        { Slug = vacancy.Slug };

        //Act
        var response = await _httpClient.GetAsync($"/api/trainings/{query.Slug}");

        //Assert
        response.EnsureSuccessStatusCode();
        var trainingVacancy = await response.Content.ReadFromJsonAsync<TrainingVacancyDetailedResponse>();

        Assert.That(trainingVacancy.IsApplied, Is.True);


    }
    [Test]
    public async Task IsApplied_ShouldBeFalse_WhenUserDidNotApply()
    {
        RunAsDefaultUserAsync("testuser");
        //Arrange 
        var vacancy = await AddJobVacancyAsync("Another cool training");

        var query = new GetTrainingsVacancyBySlugQuery
        { Slug = vacancy.Slug };

        //Act
        var response = await _httpClient.GetAsync($"/api/trainings/{query.Slug}");

        //Assert
        response.EnsureSuccessStatusCode();
        var trainingVacancy = await response.Content.ReadFromJsonAsync<TrainingVacancyDetailedResponse>();

        Assert.That(trainingVacancy.IsApplied, Is.False);


    }

    protected async Task<Guid> AddVacancyCategoryAsync(string name)
    {
        var vacancyCategory = new VacancyCategory
        {
            Name = name,
            Id = Guid.NewGuid(),
        };
        await AddAsync(vacancyCategory);
        return vacancyCategory.Id;
    }

    protected async Task<TrainingVacancy> AddJobVacancyAsync(string title)
    {
        var trainingVacancy = new TrainingVacancy
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = "hello world",
            ShortDescription = "hello world",
            PublishDate = DateTime.Now,
            CategoryId = await AddVacancyCategoryAsync("training"),
            Slug = title.ToLower().Replace(" ", "-")
        };
        await AddAsync(trainingVacancy);
        return trainingVacancy;
    }
}