using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Trainings.Command;
public class CreateTrainingVacancyCommandTest : Testing
{
    [Test]
    public async Task CreateTrainingVacancyCommand_CreatingTrainingVacancyWithVacancyQuestions_Success()
    {
        var trainingVacancy = new CreateTrainingVacancyCommand
        {
            Title = "Good Job!",
            Description = "Hey dude!",
            ShortDescription = "Hello",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("jobvacancy"),
            Duration = 2,
            VacancyQuestions = new List<VacancyQuestionDto> { new VacancyQuestionDto { Question = "What is your English proficiency level?" } },
            Fees = 5
        };

        RunAsReviewerAsync();
        var response = await _httpClient.PostAsJsonAsync("/api/trainings", trainingVacancy);

        response.EnsureSuccessStatusCode();

        response.Should().NotBeNull();
    }
    [Test]
    public async Task CreateTrainingVacancyCommand_CreatingTrainingVacancyWithVacancyTask_Success()
    {
        var trainingVacancy = new CreateTrainingVacancyCommand
        {
            Title = "Good Job!",
            Description = "Hey dude!",
            ShortDescription = "Hello",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("jobvacancy"),
            Duration = 2,
            VacancyQuestions = new List<VacancyQuestionDto> { new VacancyQuestionDto { Question = "What is your English proficiency level?" } },
            VacancyTasks = new List<VacancyTaskDto>() { new VacancyTaskDto {
            Title= "Create a function",
            Description= "Create a function Sum(a, b)",
            Template= "static class Function { //TODO: Create a function here }",
            Test= "using NUnit.Framework; class FunctionTests { [Test] public void FunctionTest(){int a = 100;int b = 100;int summ = Function.Sum(a, b);Assert.AreEqual(200, summ, 0);}}" } },
            Fees = 5
        };

        RunAsReviewerAsync();
        var response = await _httpClient.PostAsJsonAsync("/api/trainings", trainingVacancy);

        response.EnsureSuccessStatusCode();

        response.Should().NotBeNull();
    }
    async Task<Guid> AddVacancyCategory(string name)
    {
        var vacancyCategory = new VacancyCategory
        {
            Name = name,
            Id = Guid.NewGuid(),
        };
        await AddAsync(vacancyCategory);
        return vacancyCategory.Id;
    }
}
