using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.IntegrationTests.VacancyCategories.GetCreate;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Trainings.Queries;

public class GetAllTrainingsVacancyQuery : TrainingsContext
{
  
    [Test]
    public async Task GetAllTrainingsVacancyQuery_ReturnsTrainingsVacancies()
    {
        await GetTraining("C#", DateTime.Now.AddDays(1));
        await GetTraining("Pyton", DateTime.Now.AddDays(1));
        await GetTraining("F#", DateTime.Now.AddDays(1));
        await GetTraining("C++", DateTime.Now.AddDays(1));
        //Act
        RunAsReviewerAsync();
        var response = await _httpClient.GetAsync("/api/Trainings");
        //Assert
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var a = await response.Content.ReadAsStringAsync();
        var trainingVacancies = await response.Content.ReadFromJsonAsync<PagedList<TrainingVacancyListDto>>();
        Assert.That(trainingVacancies, Is.Not.Null);
        Assert.That(trainingVacancies.Items, Is.Not.Empty);
    }

    [Test]
    public async Task GetAllTrainingsVacancyQuery_ReturnsEmptyList_WhenNoTrainingsExist()
    {
        // Arrange
        var category = new CategoryContext();
        var newTraining = new TrainingVacancy
        {
            Title = "cccc",
            Description = "Hello",
            ShortDescription = "Hi",
            PublishDate = DateTime.Now.AddDays(-5),
            EndDate = DateTime.Now.AddDays(-2),
            CategoryId = await category.GetCategoryId("trainingVacancy"),
            Duration = 10,
            Fees = 100,
            VacancyQuestions = new List<VacancyQuestion>
            {
                new VacancyQuestion
                {
                    Id = Guid.NewGuid(),
                    Question = "What is your English proficiency level?"
                }
            },
        };
        await AddAsync(newTraining);
        // Act
        RunAsReviewerAsync();
        var response = await _httpClient.GetAsync("/api/Trainings");
        // Assert
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var trainingVacancies = await response.Content.ReadFromJsonAsync<PagedList<TrainingVacancyListDto>>();
        Assert.That(trainingVacancies, Is.Not.Null);
        var specificTraining = trainingVacancies.Items.Find(t => t.EndDate < DateTime.Now);
        Assert.That(specificTraining, Is.Not.Null);
    }

    [Test]
    public async Task GetAllTrainingVacanciesQuery_ReturnsTrainingVacanciesCount2_ForApplicant()
    {
        ResetState();
        await GetTraining("training1", DateTime.Now.AddDays(2));
        await GetTraining("training2", DateTime.Now.AddDays(3));
        await GetTraining("training3", DateTime.Now.AddDays(-1));

        RunAsDefaultUserAsync("applicant");
        //Act
        var response = await _httpClient.GetAsync("/api/Trainings");

        //Assert
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var trainingVacancies = await response.Content.ReadFromJsonAsync<PagedList<TrainingVacancyListDto>>();

        Assert.That(trainingVacancies, Is.Not.Null);
        Assert.That(trainingVacancies.Items.Count == 2);
    }

    [Test]
    public async Task GetAllTrainingVacanciesQuery_ReturnsTrainingVacanciesCount3_ForReviewer()
    {
        ResetState();
        await GetTraining("training1", DateTime.Now.AddDays(2));
        await GetTraining("training2", DateTime.Now.AddDays(3));
        await GetTraining("training3", DateTime.Now.AddDays(-1));

        RunAsReviewerAsync();
        //Act
        var response = await _httpClient.GetAsync("/api/Trainings");

        //Assert
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var trainingVacancies = await response.Content.ReadFromJsonAsync<PagedList<TrainingVacancyListDto>>();

        Assert.That(trainingVacancies, Is.Not.Null);
        Assert.That(trainingVacancies.Items.Count == 3);
    }
}