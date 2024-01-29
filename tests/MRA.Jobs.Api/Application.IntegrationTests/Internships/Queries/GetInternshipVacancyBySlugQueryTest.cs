using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Internships.Queries;

public class GetInternshipVacancyBySlugQueryTest : InternshipsContext
{
    [Test]
    public async Task GetInternshipVacancyBySlugQuery_IfNotFound_ReturnNotFoundInternshipVacancySlug()
    {
        // Arrange
        var query = new GetInternshipVacancyBySlugQuery { Slug = "winter-internships" };

        // Act
        var response = await _httpClient.GetAsync($"/api/internships/{query.Slug}");

        // Assert
        Assert.That(HttpStatusCode.NotFound == response.StatusCode);
    }

    [Test]
    public async Task GetInternshipVacancyBySlug_IfFound_ReturnInternshipVacancy()
    {
        //Arrange 
        var query = new GetInternshipVacancyBySlugQuery
        { Slug = (await GetInternship("Autumn Internships")).Slug };

        //Act
        var response = await _httpClient.GetAsync($"/api/internships/{query.Slug}");

        //Assert
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var internshipVacancy = await response.Content.ReadFromJsonAsync<InternshipVacancy>();
        Assert.That(query.Slug == internshipVacancy.Slug);
    }

    [Test]
    public async Task IsApplied_ShouldBeTrue_WhenUserApplied()
    {
        RunAsDefaultUserAsync("testuser");
        //Arrange 
        var vacancy = await AddJobVacancyAsync("Cool internship");

        Domain.Entities.Application application = new()
        {
            ApplicantId = GetCurrentUserId(),
            ApplicantUsername = "testuser",    
            VacancyId = vacancy.Id,
            Slug = "testuser-cool-internship"
        };

        await AddAsync(application);

        var query = new GetInternshipVacancyBySlugQuery
        { Slug = vacancy.Slug };

        //Act
        var response = await _httpClient.GetAsync($"/api/internships/{query.Slug}");

        //Assert
        response.EnsureSuccessStatusCode();
        var internshipVacancy = await response.Content.ReadFromJsonAsync<InternshipVacancyResponse>();

        Assert.That(internshipVacancy.IsApplied, Is.True);


    }
    [Test]
    public async Task IsApplied_ShouldBeFalse_WhenUserDidNotApply()
    {
        RunAsDefaultUserAsync("testuser");
        //Arrange 
        var vacancy = await AddJobVacancyAsync("Another cool internship");

        var query = new GetInternshipVacancyBySlugQuery
        { Slug = vacancy.Slug };

        //Act
        var response = await _httpClient.GetAsync($"/api/internships/{query.Slug}");

        //Assert
        response.EnsureSuccessStatusCode();
        var internshipVacancy = await response.Content.ReadFromJsonAsync<InternshipVacancyResponse>();

        Assert.That(internshipVacancy.IsApplied, Is.False);


    }

    protected async Task<Guid> AddVacancyCategoryAsync(string name)
    {
        var vacancyCategory = new VacancyCategory
        {
            Name = name,
            Id = Guid.NewGuid(),
        };
        await AddAsync(vacancyCategory);
        return vacancyCategory.Id;
    }

    protected async Task<InternshipVacancy> AddJobVacancyAsync(string title)
    {
        var internshipVacancy = new InternshipVacancy
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = "hello world",
            ShortDescription = "hello world",
            Stipend = 100,
            Duration = 3,
            PublishDate = DateTime.Now,
            ApplicationDeadline = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategoryAsync("internship"),
            Slug = title.ToLower().Replace(" ", "-")
        };
        await AddAsync(internshipVacancy);
        return internshipVacancy;
    }
}