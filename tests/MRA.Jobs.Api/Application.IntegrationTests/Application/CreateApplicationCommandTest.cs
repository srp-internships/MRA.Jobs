using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Application;

public class CreateApplicationCommandTest : Testing
{
    private static readonly Random Random = new();

    [Test]
    public async Task CreateApplicationCommand_CreatingApplication_Success()
    {
        var vacancyId = await AddJobVacancy("foobar2");
        var testSubmit = new CreateApplicationCommand
        {
            VacancyId = vacancyId,
            CoverLetter = RandomString(150),
            Cv =
            {
                IsUploadCvMode = true,
                CvBytes = new byte[] { 1, 2, 3 },
                FileName = "213.bytes"
            }
        };
        RunAsDefaultUserAsync();
        var response = await _httpClient.PostAsJsonAsync("/api/applications", testSubmit);

        response.EnsureSuccessStatusCode();

        var responseGuid = await response.Content.ReadAsStringAsync();

        responseGuid.Should().NotBeEmpty();
    }

    [Test]
    public async Task CreateApplicationCommand_CreateApplicationWithVacancyQuestions_Success()
    {
        var vacancyId = await AddJobVacancy("foobar");
        var testSubmit = new CreateApplicationCommand
        {
            VacancyId = vacancyId,
            CoverLetter = RandomString(200),
            VacancyResponses = new List<VacancyResponseDto>
            {
                new() { VacancyQuestion = new VacancyQuestionDto { Question = "How old are you?" }, Response = "56" },
                new()
                {
                    VacancyQuestion = new VacancyQuestionDto { Question = "What is your English proficiency level?" },
                    Response = "Beginner"
                }
            },
            Cv =
            {
                IsUploadCvMode = true,
                CvBytes = new byte[] { 1, 2, 3 },
                FileName = "213.bytes"
            }
        };

        RunAsDefaultUserAsync();
        var response = await _httpClient.PostAsJsonAsync("/api/applications", testSubmit);

        response.EnsureSuccessStatusCode();

        var responseGuid = await response.Content.ReadAsStringAsync();

        responseGuid.Should().NotBeEmpty();
    }

    [Test]
    public async Task CreateApplicationCommand_CreateApplicationWithVacancyTask_Success()
    {
        var vacancyId = await AddJobVacancy("foobar11");
        var testSubmit = new CreateApplicationCommand
        {
            VacancyId = vacancyId,
            CoverLetter = RandomString(200),
            VacancyResponses = new List<VacancyResponseDto>
            {
                new() { VacancyQuestion = new VacancyQuestionDto { Question = "How old are you?" }, Response = "56" },
                new()
                {
                    VacancyQuestion = new VacancyQuestionDto { Question = "What is your English proficiency level?" },
                    Response = "Beginner"
                }
            },
            TaskResponses = new List<TaskResponseDto>
            {
                new()
                {
                    Code = "static class Function {public static int Sum(int a, int b){return a + b;}",
                    TaskId = Guid.Empty
                },
                new()
                {
                    Code = "static class Function {public static int Sum(int a, int b){return a - b;}",
                    TaskId = Guid.Empty
                }
            },
            Cv =
            {
                IsUploadCvMode = true,
                CvBytes = new byte[] { 1, 2, 3 },
                FileName = "213.bytes"
            }
        };

        RunAsDefaultUserAsync();
        var response = await _httpClient.PostAsJsonAsync("/api/applications", testSubmit);

        response.EnsureSuccessStatusCode();

        var responseGuid = await response.Content.ReadAsStringAsync();

        responseGuid.Should().NotBeEmpty();
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

    async Task<Guid> AddJobVacancy(string title)
    {
        var internshipVacancy = new InternshipVacancy
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = RandomString(50),
            ShortDescription = RandomString(10),
            Stipend = 100,
            Duration = 3,
            PublishDate = DateTime.Now,
            ApplicationDeadline = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("internship"),
            Slug = title.ToLower().Replace(" ", "-")
        };
        await AddAsync(internshipVacancy);
        return internshipVacancy.Id;
    }

    [Test]
    public async Task Handle_DuplicateApplyForUser_ReturnsConflict()
    {
        var jobId = await AddJobVacancy("newVacancyForDuplicateTest");

        RunAsDefaultUserAsync();

        var createCommand = new CreateApplicationCommand
        {
            CoverLetter = RandomString(200),
            VacancyId = jobId,
            VacancyResponses = null,
            Cv =
            {
                IsUploadCvMode = true,
                CvBytes = new byte[] { 1, 2, 3 },
                FileName = "213.bytes"
            }
        };

        await _httpClient.PostAsJsonAsync("/api/applications", createCommand);
        var response = await _httpClient.PostAsJsonAsync("/api/applications", createCommand);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}