using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Jobs.Command;
public class CreateJobVacancyCommandTest : Testing
{

    private static Random random = new Random();

    [Test]
    public async Task CreateJobVacancyCommand_CreatingJobVacancyWithQuestions_Success()
    {
        RunAsReviewerAsync();
        var jobVacancy = new CreateJobVacancyCommand {            
            Title = "Cool Job",
            RequiredYearOfExperience = 5,
            Description = RandomString(10),
            ShortDescription = RandomString(10),
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("jobvacancy"),
            VacancyQuestions = new List<VacancyQuestionDto> { new VacancyQuestionDto { Question = "What is your English proficiency level?" } },
            WorkSchedule = Contracts.Dtos.Enums.ApplicationStatusDto.WorkSchedule.FullTime
        };
        var response = await _httpClient.PostAsJsonAsync("/api/jobs", jobVacancy);

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

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
