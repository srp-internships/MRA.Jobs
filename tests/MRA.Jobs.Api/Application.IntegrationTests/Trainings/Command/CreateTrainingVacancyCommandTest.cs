using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Trainings.Command;
public class CreateTrainingVacancyCommandTest : Testing
{
    [Test]
    public async Task CreateTrainingVacancyCommand_CreatingTrainingVacancyWithVacancyQuestions_Success()
    {
        RunAsAdministratorAsync();
        var trainingVacancy = new CreateTrainingVacancyCommand
        {
            Title = "Cool Job",
            Description = "Hello",
            ShortDescription = "Hi",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("jobvacancy"),
            Duration = 10,
            Fees = 100,
            VacancyQuestions = new List<VacancyQuestionDto> { new VacancyQuestionDto { Question = "What is your English proficiency level?" } },
        };

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
