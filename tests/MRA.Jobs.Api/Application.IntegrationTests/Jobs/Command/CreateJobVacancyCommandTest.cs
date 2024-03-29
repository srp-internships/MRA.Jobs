﻿using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Jobs.Command;

public class CreateJobVacancyCommandTest : Testing
{
    private static readonly Random Random = new();

    [Test]
    public async Task CreateJobVacancyCommand_CreatingJobVacancy_Slug_Translate_KrillToLatin_Success()
    {
        RunAsReviewerAsync();
        var jobVacancy = new CreateJobVacancyCommand
        {
            Title = "Бэкенд разработчик",
            RequiredYearOfExperience = 5,
            Description = RandomString(10),
            ShortDescription = RandomString(10),
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("jobvacancy"),
            WorkSchedule = Contracts.Dtos.Enums.ApplicationStatusDto.WorkSchedule.FullTime
        };
        var response = await _httpClient.PostAsJsonAsync("/api/jobs", jobVacancy);
        response.EnsureSuccessStatusCode();
        response.Should().NotBeNull();
        
        const string slug = "bekend-razrabotchik-1-1";

        var job = await FindFirstOrDefaultAsync<JobVacancy>(j=>j.Slug==slug);
        job.Should().NotBeNull();
       
        Assert.That(jobVacancy.Title==job.Title);
    }
    
    [Test]
    public async Task CreateJobVacancyCommand_CreatingJobVacancyWithQuestions_Success()
    {
        RunAsReviewerAsync();
        var jobVacancy = new CreateJobVacancyCommand
        {
            Title = "Cool Job",
            RequiredYearOfExperience = 5,
            Description = RandomString(10),
            ShortDescription = RandomString(10),
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("jobvacancy"),
            VacancyQuestions = new List<VacancyQuestionDto>
                { new() { Question = "What is your English proficiency level?" } },
            WorkSchedule = Contracts.Dtos.Enums.ApplicationStatusDto.WorkSchedule.FullTime
        };
        var response = await _httpClient.PostAsJsonAsync("/api/jobs", jobVacancy);

        response.EnsureSuccessStatusCode();

        response.Should().NotBeNull();
    }
    
    [Test]
    public async Task CreateJobVacancyCommand_CreatingJobVacancyWithTask_Success()
    {
        RunAsReviewerAsync();
        var jobVacancy = new CreateJobVacancyCommand
        {
            Title = "Cool Job5",
            RequiredYearOfExperience = 5,
            Description = RandomString(10),
            ShortDescription = RandomString(10),
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("jobvacancy"),
            VacancyQuestions = new List<VacancyQuestionDto> { new VacancyQuestionDto { Question = "What is your English proficiency level?" } },
            VacancyTasks = new List<VacancyTaskDto>() { new VacancyTaskDto {
              Title= "Create a function",
              Description= "Create a function Sum(a, b)",
              Template= "static class Function { //TODO: Create a function here }",
              Test= "using NUnit.Framework; class FunctionTests { [Test] public void FunctionTest(){int a = 100;int b = 100;int summ = Function.Sum(a, b);Assert.AreEqual(200, summ, 0);}}" } },
            WorkSchedule = Contracts.Dtos.Enums.ApplicationStatusDto.WorkSchedule.FullTime
        };
        var response = await _httpClient.PostAsJsonAsync("/api/jobs", jobVacancy);

        response.EnsureSuccessStatusCode();

        response.Should().NotBeNull();
    }
    [Test]
    public async Task CreateJobVacancyCommand_CreatingJobVacancyDuplicate_ConflictException()
    {
        RunAsReviewerAsync();
        var jobVacancy = await AddJob("Cool Jobs");
        var jobVacancy1 =await AddJob("Cool Jobs");
        await _httpClient.PostAsJsonAsync("/api/jobs", jobVacancy);
        var response = await _httpClient.PostAsJsonAsync("/api/jobs", jobVacancy1);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task CreateJobVacancy_ValidRequest_FillDatabase()
    {
        RunAsReviewerAsync();
        var jobVacancy = new CreateJobVacancyCommand
        {
            Title = "Cool Job1",
            RequiredYearOfExperience = 5,
            Description = RandomString(10),
            ShortDescription = RandomString(10),
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("jobVacancy1"),
            VacancyQuestions = new List<VacancyQuestionDto>
                { new() { Question = "What is your English proficiency level?1" } },
            WorkSchedule = Contracts.Dtos.Enums.ApplicationStatusDto.WorkSchedule.FullTime
        };
        await _httpClient.PostAsJsonAsync("/api/jobs", jobVacancy);
        
        var databaseVacancy = await FindFirstOrDefaultAsync<JobVacancy>(s =>
            s.CategoryId == jobVacancy.CategoryId &&
            s.Title == jobVacancy.Title &&
            s.Description == jobVacancy.Description);
        databaseVacancy.Should().NotBeNull();

        databaseVacancy.CreatedAt.Should().NotBe(null);
        databaseVacancy.CreatedByEmail.Should().NotBeNull();
        databaseVacancy.CreatedBy.Should().NotBeEmpty();
    }

    public async Task<CreateJobVacancyCommand> AddJob(string title)
    {
        return new CreateJobVacancyCommand
        {
            Title = title,
            RequiredYearOfExperience = 5,
            Description = RandomString(10),
            ShortDescription = RandomString(10),
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("jobvacancy"),
            WorkSchedule = Contracts.Dtos.Enums.ApplicationStatusDto.WorkSchedule.FullTime
        };
    }
    private async Task<Guid> AddVacancyCategory(string name)
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