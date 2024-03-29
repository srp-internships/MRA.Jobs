﻿using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
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
            VacancyQuestions = new List<VacancyQuestionDto>
                { new VacancyQuestionDto { Question = "What is your English proficiency level?" } },
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
            Title = "Good Job10!",
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
    [Test]
    public async Task CreateTrainingVacancyCommand_CreateTrainingDuplicate_ConflictException()
    {
        RunAsReviewerAsync();
        var Training = await AddTraining("Good Job11");
        var Training1 = await AddTraining("Good Job11");
        await _httpClient.PostAsJsonAsync("/api/trainings", Training);
        var response = await _httpClient.PostAsJsonAsync("/api/trainings", Training1);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }
    [Test]
    public async Task CreateTrainingVacancy_ValidRequest_ShouldFillDatabase()
    {
        RunAsAdministratorAsync();
        var training = new CreateTrainingVacancyCommand
        {
            Title = "Good Job!1",
            Description = "Hey dude!1",
            ShortDescription = "Hello1",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("jobVacancy"),
            Duration = 2,
            VacancyQuestions = new List<VacancyQuestionDto>
                { new() { Question = "What is your English proficiency level?1" } },
            Fees = 5
        };
        await _httpClient.PostAsJsonAsync("/api/trainings", training);
        
        var databaseVacancy = await FindFirstOrDefaultAsync<TrainingVacancy>(s =>
            s.CategoryId == training.CategoryId &&
            s.Title == training.Title &&
            s.Description == training.Description);
        databaseVacancy.Should().NotBeNull();

        databaseVacancy.CreatedAt.Should().NotBe(null);
        databaseVacancy.CreatedByEmail.Should().NotBeNull();
        databaseVacancy.CreatedBy.Should().NotBeEmpty();
    }
    public async Task<CreateTrainingVacancyCommand> AddTraining(string title)
    {
        return new CreateTrainingVacancyCommand
        {
            Title = title,
            Description = "Hey dude!",
            ShortDescription = "Hello",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("jobvacancy"),
            Duration = 2,
            Fees = 5
        };
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