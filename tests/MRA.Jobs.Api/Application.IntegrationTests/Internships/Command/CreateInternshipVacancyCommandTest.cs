using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Internships.Command;

public class CreateInternshipVacancyCommandTest : Testing
{
    [Test]
    public async Task CreateInternshipVacancyCommand_CreatingInternshipVacancyCommandWithVacancyQuestions_Success()
    {
        RunAsAdministratorAsync();
        var internshipVacancy = new CreateInternshipVacancyCommand
        {
            Title = "Cool Job",
            Description = "Hello",
            ShortDescription = "Hi",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("internshipVacancy"),
            ApplicationDeadline = DateTime.Now.AddDays(20),
            Duration = 10,
            Stipend = 100,
            VacancyQuestions = new List<VacancyQuestionDto>
                { new() { Question = "What is your English proficiency level?" } }
        };

        var response = await _httpClient.PostAsJsonAsync("/api/internships", internshipVacancy);

        response.EnsureSuccessStatusCode();

        response.Should().NotBeNull();
    }

    [Test]
    public async Task CreateInternshipVacancy_ValidRequest_ShouldFillDatabase()
    {
        RunAsAdministratorAsync();
        var internshipVacancy = new CreateInternshipVacancyCommand
        {
            Title = "Cool Job1",
            Description = "Hello1",
            ShortDescription = "Hi1",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("internshipVacancy1"),
            ApplicationDeadline = DateTime.Now.AddDays(20),
            Duration = 10,
            Stipend = 100,
            VacancyQuestions = new List<VacancyQuestionDto>
                { new() { Question = "What is your English proficiency level?1" } }
        };
        await _httpClient.PostAsJsonAsync("/api/internships", internshipVacancy);
        
        var databaseVacancy = await FindFirstOrDefaultAsync<InternshipVacancy>(s =>
            s.CategoryId == internshipVacancy.CategoryId && 
            s.Title == internshipVacancy.Title &&
            s.Description == internshipVacancy.Description);
        databaseVacancy.Should().NotBeNull();

        databaseVacancy.CreatedAt.Should().NotBe(null);
        databaseVacancy.CreatedByEmail.Should().NotBeNull();
        databaseVacancy.CreatedBy.Should().NotBeEmpty();
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