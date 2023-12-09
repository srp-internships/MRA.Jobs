using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Dtos;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Application;

public class CreateApplicationCommandTest : CreateApplicationTestsBase
{
    [Test]
    public async Task CreateApplicationCommand_CreatingApplication_Success()
    {
        var vacancyId = await AddJobVacancyAsync("foobar2");
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
        RunAsDefaultUserAsync("applicant1");
        var response = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, testSubmit);

        response.EnsureSuccessStatusCode();

        var responseGuid = await response.Content.ReadAsStringAsync();

        responseGuid.Should().NotBeEmpty();
    }

    [Test]
    public async Task CreateApplicationCommand_CreateApplicationWithVacancyQuestions_Success()
    {
        var vacancyId = await AddJobVacancyAsync("foobar");
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

        RunAsDefaultUserAsync("applicant1");
        var response = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, testSubmit);

        response.EnsureSuccessStatusCode();

        var responseGuid = await response.Content.ReadAsStringAsync();

        responseGuid.Should().NotBeEmpty();
    }

    [Test]
    public async Task CreateApplicationCommand_CreateApplicationWithVacancyTask_Success()
    {
        var vacancyId = await AddJobVacancyAsync("foobar11");
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

        RunAsDefaultUserAsync("applicant1");
        var response = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, testSubmit);

        response.EnsureSuccessStatusCode();

        var responseGuid = await response.Content.ReadAsStringAsync();

        responseGuid.Should().NotBeEmpty();
    }


    [Test]
    public async Task Handle_DuplicateApplyForUser_ReturnsConflict()
    {
        var jobId = await AddJobVacancyAsync("newVacancyForDuplicateTest");

        RunAsDefaultUserAsync("applicant1");

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

        await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, createCommand);
        var response = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, createCommand);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}