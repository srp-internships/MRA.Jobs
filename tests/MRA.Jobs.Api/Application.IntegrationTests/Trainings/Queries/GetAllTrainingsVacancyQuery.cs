using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Trainings.Queries;
public class GetAllTrainingsVacancyQuery : Testing
{
    private TrainingsContext _context;

    [SetUp]
    public void SetUp()
    {
        _context = new TrainingsContext();
    }
    [Test]
    public async Task GetAllTrainingsVacancyQuery_ReturnsTrainingsVacancies()
    {
        await _context.GetTraining("C#");
        await _context.GetTraining("Pyton");
        await _context.GetTraining("F#");
        await _context.GetTraining("C++");
        //Act
        RunAsReviewerAsync();
        var response = await _httpClient.GetAsync("/api/Trainings");
        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var a = await response.Content.ReadAsStringAsync();
        var trainingVacancies = await response.Content.ReadFromJsonAsync<PagedList<TrainingVacancyListDto>>();
        Assert.IsNotNull(trainingVacancies);
        Assert.IsNotEmpty(trainingVacancies.Items);
    }
    [Test]
    public async Task GetAllTrainingsVacancyQuery_ReturnsSpecificTraining()
    {
        // Arrange
        await _context.GetTraining("C#");
        await _context.GetTraining("Pyton");
        await _context.GetTraining("F#");
        await _context.GetTraining("C++");
        var expectedTrainingName = "C#";

        // Act
        RunAsReviewerAsync();
        var response = await _httpClient.GetAsync("/api/Trainings");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var trainingVacancies = await response.Content.ReadFromJsonAsync<PagedList<TrainingVacancyListDto>>();
        Assert.IsNotNull(trainingVacancies);
        Assert.IsNotEmpty(trainingVacancies.Items);

        var specificTraining = trainingVacancies.Items.Find(t => t.Title == expectedTrainingName);
        Assert.IsNotNull(specificTraining);
    }
    [Test]
    public async Task ReturnsSpecificTraining_WhenTrainingExists()
    {
        // Arrange
        await _context.GetTraining("C#");
        await _context.GetTraining("Python");
        await _context.GetTraining("F#");
        await _context.GetTraining("C++");
        var expectedTrainingName = "C#";

        // Act
        RunAsReviewerAsync();
        var response = await _httpClient.GetAsync("/api/Trainings");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var trainingVacancies = await response.Content.ReadFromJsonAsync<PagedList<TrainingVacancyListDto>>();
        Assert.IsNotNull(trainingVacancies);
        Assert.IsNotEmpty(trainingVacancies.Items);

        var specificTraining = trainingVacancies.Items.Find(t => t.Title == expectedTrainingName);
        Assert.IsNotNull(specificTraining);
        Assert.AreEqual(expectedTrainingName, specificTraining.Title);
        Assert.IsTrue(specificTraining.Duration > 0);
        Assert.IsTrue(specificTraining.Description!="");
    }
    [Test]
    public async Task GetAllTrainingsVacancyQuery_ReturnsForbiddenStatusCode_WhenUserIsNotAuthorized()
    {
        await _context.GetTraining("C#");
        await _context.GetTraining("Pyton");
        await _context.GetTraining("F#");
        await _context.GetTraining("C++");
        //Act
        RunAsDefaultUserAsync();
        var response = await _httpClient.GetAsync("/api/Trainings");
        //Assert
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }
    [Test]
    public async Task GetAllTrainingsVacancyQuery_ReturnsEmptyList_WhenNoTrainingsExist()
    {
        // Arrange
        // Не добавляем тренировочные места
        // Act
        RunAsReviewerAsync();
        var response = await _httpClient.GetAsync("/api/Trainings");
        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var trainingVacancies = await response.Content.ReadFromJsonAsync<PagedList<TrainingVacancyListDto>>();
        Assert.IsNotNull(trainingVacancies);
    }
    [Test]
    public async Task GetAllTrainingsVacancyQuery_ReturnsNullForNonexistentTraining()
    {
        // Arrange
        await _context.GetTraining("Python");
        await _context.GetTraining("F#");
        await _context.GetTraining("C++");
        var expectedTrainingName = "C#";

        // Act
        RunAsReviewerAsync();
        var response = await _httpClient.GetAsync("/api/Trainings");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var trainingVacancies = await response.Content.ReadFromJsonAsync<PagedList<TrainingVacancyListDto>>();
        Assert.IsNotNull(trainingVacancies);
        Assert.IsNotEmpty(trainingVacancies.Items);

        var specificTraining = trainingVacancies.Items.Find(t => t.Title == expectedTrainingName);
        Assert.IsNull(specificTraining);
    }

}

