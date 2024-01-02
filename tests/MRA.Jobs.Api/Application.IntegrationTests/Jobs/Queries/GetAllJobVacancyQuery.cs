using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Jobs.Queries;

public class GetAllJobVacancyQuery : Testing
{
    private JobsContext _context;

    [SetUp]
    public void SetUp()
    {
        _context = new JobsContext();
    }


    [Test]
    public async Task GetAllJobVacancyQuery_ReturnsJobVacancies()
    {
        await _context.GetJob("job1", DateTime.Now.AddDays(2));
        await _context.GetJob("job2", DateTime.Now.AddDays(3));

        //Act
        var response = await _httpClient.GetAsync("/api/jobs");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var jobVacancies = await response.Content.ReadFromJsonAsync<PagedList<JobVacancyListDto>>();

        Assert.IsNotNull(jobVacancies);
        Assert.IsNotEmpty(jobVacancies.Items);
    }

    [Test]
    public async Task GetAllJobVacancyQuery_ReturnsJobVacanciesCount2_ForApplicant()
    {
        ResetState();
        await _context.GetJob("job1", DateTime.Now.AddDays(2));
        await _context.GetJob("job2", DateTime.Now.AddDays(3));
        await _context.GetJob("job3", DateTime.Now.AddDays(-1));

        RunAsDefaultUserAsync("applicant");
        //Act
        var response = await _httpClient.GetAsync("/api/jobs");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var jobVacancies = await response.Content.ReadFromJsonAsync<PagedList<JobVacancyListDto>>();

        Assert.IsNotNull(jobVacancies);
        Assert.AreEqual(jobVacancies.Items.Count, 2);
    }
    
    [Test]
    public async Task GetAllJobVacancyQuery_ReturnsJobVacanciesCount3_ForReviewer()
    {
        ResetState();
        await _context.GetJob("job1", DateTime.Now.AddDays(2));
        await _context.GetJob("job2", DateTime.Now.AddDays(3));
        await _context.GetJob("job3", DateTime.Now.AddDays(-1));

        RunAsReviewerAsync();
        //Act
        var response = await _httpClient.GetAsync("/api/jobs");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var jobVacancies = await response.Content.ReadFromJsonAsync<PagedList<JobVacancyListDto>>();

        Assert.IsNotNull(jobVacancies);
        Assert.AreEqual(jobVacancies.Items.Count, 3);
    }
}