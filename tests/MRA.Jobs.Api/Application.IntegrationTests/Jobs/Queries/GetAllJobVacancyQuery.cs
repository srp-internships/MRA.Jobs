using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Jobs.Queries;

public class GetAllJobVacancyQuery : JobsContext
{
    [Test]
    public async Task GetAllJobVacancyQuery_ReturnsJobVacancies()
    {
        await GetJob("job11", DateTime.Now.AddDays(2));
        await GetJob("job12", DateTime.Now.AddDays(3));

        //Act
        var response = await _httpClient.GetAsync("/api/jobs");

        //Assert
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var jobVacancies = await response.Content.ReadFromJsonAsync<PagedList<JobVacancyListDto>>();

        Assert.That(jobVacancies, Is.Not.Null);
        Assert.That(jobVacancies.Items, Is.Not.Empty);
    }

    [Test]
    public async Task GetAllJobVacancyQuery_ReturnsJobVacanciesCount2_ForApplicant()
    {
        ResetState();
        await GetJob("job3", DateTime.Now.AddDays(2));
        await GetJob("job4", DateTime.Now.AddDays(3));
        await GetJob("job5", DateTime.Now.AddDays(-1));

        //Act
        var response = await _httpClient.GetAsync("/api/jobs");

        //Assert
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var jobVacancies = await response.Content.ReadFromJsonAsync<PagedList<JobVacancyListDto>>();

        Assert.That(jobVacancies, Is.Not.Null);
        Assert.That(jobVacancies.Items.Count == 2);
    }

    [Test]
    public async Task GetAllJobVacancyQuery_ReturnsJobVacanciesCount3_ForReviewer()
    {
        ResetState();
        await GetJob("job3", DateTime.Now.AddDays(2));
        await GetJob("job4", DateTime.Now.AddDays(3));
        await GetJob("job5", DateTime.Now.AddDays(-1));

        RunAsReviewerAsync();
        //Act
        var response = await _httpClient.GetAsync("/api/jobs");

        //Assert
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var jobVacancies = await response.Content.ReadFromJsonAsync<PagedList<JobVacancyListDto>>();

        Assert.That(jobVacancies, Is.Not.Null);
        Assert.That(jobVacancies.Items.Count == 3);
    }
}