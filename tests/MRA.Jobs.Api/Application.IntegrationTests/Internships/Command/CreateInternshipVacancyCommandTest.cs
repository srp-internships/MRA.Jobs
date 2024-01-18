using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
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
    public async Task CreateInternshipVacancyCommand_CreatingInternshipVacancyCommandWithVacancyTask_Success()
    {
        RunAsAdministratorAsync();
        var internshipVacancy = new CreateInternshipVacancyCommand
        {
            Title = "Cool Jobs",
            Description = "Hello word",
            ShortDescription = "Hi",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("internshipvacancy"),
            ApplicationDeadline = DateTime.Now.AddDays(20),
            Duration = 10,
            Stipend = 100,
            VacancyQuestions = new List<VacancyQuestionDto> { new VacancyQuestionDto { Question = "What is your English proficiency level?" } },
            VacancyTasks = new List<VacancyTaskDto>() { new VacancyTaskDto { Title = "Create a function", Description = "Create a function Sum(a, b)", Template = "static class Function { //TODO: Create a function here }" } }
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
    [Test]
    public async Task CreateInternshipCommand_CreatingInternshipDuplicate_ConflictException()
    {
        RunAsReviewerAsync();
        var internship = await AddInternship("Cool Jobs11");
        var internship1 =await AddInternship("Cool Jobs11");
        await _httpClient.PostAsJsonAsync("/api/internships", internship);
        var response = await _httpClient.PostAsJsonAsync("/api/internships", internship1);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }
    public async Task<CreateInternshipVacancyCommand> AddInternship(string title)
    {
       return new CreateInternshipVacancyCommand
        {
            Title = title,
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