using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobVacancyBySlug;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Jobs.Queries;

public class GetJobVacancyBySlugQueryTest : JobsContext
{
    [Test]
    public async Task GetJobVacancyBySlugQuery_IfNotFound_ReturnNotFoundJobVacancySlug()
    {
        // Arrange
        var query = new GetJobVacancyBySlugQuery { Slug = "c-sharp-developer" };

        // Act
        var response = await _httpClient.GetAsync($"/api/jobs/{query.Slug}");

        // Assert
        Assert.That(HttpStatusCode.NotFound == response.StatusCode);
    }

    [Test]
    public async Task GetJobVacancyBySlug_IfFound_ReturnJobVacancy()
    {
        //Arrange 
        var query = new GetJobVacancyBySlugQuery
        {
            Slug = (
                await GetJob("Backend Developer", DateTime.Now.AddDays(2))).Slug
        };

        //Act
        var response = await _httpClient.GetAsync($"/api/jobs/{query.Slug}");

        //Assert
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var jobVacancy = await response.Content.ReadFromJsonAsync<JobVacancy>();
        Assert.That(query.Slug== jobVacancy.Slug);
    }


    [Test]
    public async Task IsApplied_ShouldBeTrue_WhenUserApplied()
    {
        RunAsDefaultUserAsync("testuser");
        //Arrange 
        var vacancy = await AddJobVacancyAsync("Cool job");

        Domain.Entities.Application application = new()
        {
            ApplicantId = GetCurrentUserId(),
            ApplicantUsername = "testuser",
            VacancyId = vacancy.Id,
            Slug = "testuser-cool-job"
        };

        await AddAsync(application);

        var query = new GetJobVacancyBySlugQuery
        { Slug = vacancy.Slug };

        //Act
        var response = await _httpClient.GetAsync($"/api/jobs/{query.Slug}");

        //Assert
        response.EnsureSuccessStatusCode();
        var jobVacancy = await response.Content.ReadFromJsonAsync<JobVacancyDetailsDto>();

        Assert.That(jobVacancy.IsApplied, Is.True);


    }
    [Test]
    public async Task IsApplied_ShouldBeFalse_WhenUserDidNotApply()
    {
        RunAsDefaultUserAsync("testuser");
        //Arrange 
        var vacancy = await AddJobVacancyAsync("Another cool job");

        var query = new GetJobVacancyBySlugQuery
        { Slug = vacancy.Slug };

        //Act
        var response = await _httpClient.GetAsync($"/api/jobs/{query.Slug}");

        //Assert
        response.EnsureSuccessStatusCode();
        var jobVacancy = await response.Content.ReadFromJsonAsync<JobVacancyDetailsDto>();

        Assert.That(jobVacancy.IsApplied, Is.False);


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

    protected async Task<JobVacancy> AddJobVacancyAsync(string title)
    {
        var jobVacancy = new JobVacancy
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = "hello world",
            ShortDescription = "hello world",
            PublishDate = DateTime.Now,
            CategoryId = await AddVacancyCategoryAsync("job"),
            Slug = title.ToLower().Replace(" ", "-")
        };
        await AddAsync(jobVacancy);
        return jobVacancy;
    }
}