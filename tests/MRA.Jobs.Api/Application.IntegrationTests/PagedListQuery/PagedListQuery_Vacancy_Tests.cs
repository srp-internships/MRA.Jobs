using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using System.Net.Http.Json;
using System.Net;
using MRA.Jobs.Application.IntegrationTests.Jobs;
using NUnit.Framework;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.PagedList;
public class PagedListQuery_Vacancy_Tests : JobsContext
{
    [Test]
    public async Task GetAllJobVacancyQuery_WithFilters_ReturnsFilteredJobVacancies()
    {
        await GetJob("newjob1", DateTime.Now.AddDays(2));
        await GetJob("newjob2", DateTime.Now.AddDays(3));

        var filters = new Dictionary<string, string>
            {
                { "Filters", "title@=newjob1" }
            };

        var url = "/api/jobs?" + string.Join("&", filters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));

        var response = await _httpClient.GetAsync(url);

        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var jobVacancies = await response.Content.ReadFromJsonAsync<PagedList<JobVacancyListDto>>();

        Assert.That(jobVacancies, Is.Not.Null);
        Assert.That(jobVacancies.Items, Is.Not.Empty);
        Assert.That(jobVacancies.Items.All(j => j.Title.Contains("newjob1")));
    }
    [Test]
    public async Task GetJobVacancyWithInvalidFilters_ReturnsEmptyList()
    {
        await GetJob("newjob1_1", DateTime.Now.AddDays(2));
        await GetJob("newjob2_1", DateTime.Now.AddDays(3));


        var filters = new Dictionary<string, string>
            {
                { "Filters", "title@=newjob2342" }
            };

        var url = "/api/jobs?" + string.Join("&", filters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        var response = await _httpClient.GetAsync(url);

        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var jobVacancies = await response.Content.ReadFromJsonAsync<PagedList<JobVacancyListDto>>();

        Assert.That(jobVacancies, Is.Not.Null);
        Assert.That(jobVacancies.Items, Is.Empty);
    }
    [Test]
    public async Task GetJobVacancyWithPagination_ReturnsExpectedPage()
    {
        await GetJob("newjob_C-shapr", DateTime.Now.AddDays(2));
        await GetJob("newjob_Python", DateTime.Now.AddDays(3));
        await GetJob("newjob_Java", DateTime.Now.AddDays(1));
        await GetJob("newjob_JavaScript", DateTime.Now.AddDays(4));

        var parameters = new Dictionary<string, string>
        {
            { "Page", "1" },
            { "PageSize", "1" }
        };
        var url = "/api/jobs?" + string.Join("&", parameters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        var response = await _httpClient.GetAsync(url);
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var jobVacancies = await response.Content.ReadFromJsonAsync<PagedList<JobVacancyListDto>>();
        Assert.That(jobVacancies, Is.Not.Null);
        Assert.That(jobVacancies.PageSize, Is.EqualTo(1));
        Assert.That(jobVacancies.CurrentPageNumber, Is.EqualTo(1));
        Assert.That(jobVacancies.Items.Count, Is.EqualTo(1));
    }
    [Test]
    public async Task GetJobVacancyWithPagination_ReturnsExpectedPageNumber()
    {

        await GetJob("newjob_C-shapr_1", DateTime.Now.AddDays(2));
        await GetJob("newjob_Python_1", DateTime.Now.AddDays(3));
        await GetJob("newjob_Java_1", DateTime.Now.AddDays(1));
        await GetJob("newjob_JavaScript_1", DateTime.Now.AddDays(4));

        var parameters = new Dictionary<string, string>
        {
            { "Page", "2" },
            { "PageSize", "2" }
        };
        var url = "/api/jobs?" + string.Join("&", parameters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        var response = await _httpClient.GetAsync(url);
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var jobVacancies = await response.Content.ReadFromJsonAsync<PagedList<JobVacancyListDto>>();
        Assert.That(jobVacancies, Is.Not.Null);
        Assert.That(jobVacancies.PageSize, Is.EqualTo(2));
        Assert.That(jobVacancies.CurrentPageNumber, Is.EqualTo(2));
        Assert.That(jobVacancies.Items.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetJobVacancyWithFiltersAndPaginationAndTag_ReturnsFilteredJobVacancies()
    {

        await GetJob("newjob3", DateTime.Now.AddDays(2));
        var _vacancy = await GetJob("newjob4", DateTime.Now.AddDays(3));
        RunAsRoleAsync(UserRoles.Admin);
        var vacancyTag = new VacancyTag()
        {
            Tag = new Tag() { Name = "newjob" },
            VacancyId = _vacancy.Id
        };
        await AddAsync(vacancyTag);
        var parameters = new Dictionary<string, string>
            {
                { "Filters", "title@=newjob3" },
                { "Page", "1" },
                { "PageSize", "1" }
            };
        var url = "/api/jobs?" + string.Join("&", parameters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));

        var response = await _httpClient.GetAsync(url);

        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var jobVacancies = await response.Content.ReadFromJsonAsync<PagedList<JobVacancyListDto>>();
        Assert.That(jobVacancies, Is.Not.Null);
        Assert.That(jobVacancies.Items, Is.Not.Empty);
        Assert.That(jobVacancies.Items.All(j => j.Title.Contains("newjob3")));
    }
    [Test]
    public async Task GetJobVacancyWithTagsFilter_ReturnsFilteredJobVacancies()
    {
        var _vacancy= await GetJob("newjob_AddTeg", DateTime.Now.AddDays(2));
        RunAsRoleAsync(UserRoles.Admin);
        var vacancyTag = new VacancyTag()
        {
            Tag = new Tag() { Name = "new" },
            VacancyId = _vacancy.Id
        };

        await AddAsync(vacancyTag);
        //var filters = new Dictionary<string, string>
        //    {
        //        { "Filters", "title@=newjob_AddTeg" }
        //    };
        //var url = "/api/jobs?" + string.Join("&", filters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        //var respons = await _httpClient.GetAsync(url);
        var parameters = new Dictionary<string, string>
        {
            { "Tags", "new" },
        };
        var url = "/api/jobs?" + string.Join("&", parameters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        var response = await _httpClient.GetAsync(url);
        Assert.That(HttpStatusCode.OK == response.StatusCode);
         var jobVacancies = await response.Content.ReadFromJsonAsync<PagedList<JobVacancyListDto>>();
        Assert.That(jobVacancies, Is.Not.Null);
        Assert.That(jobVacancies.Items, Is.Not.Empty);
        Assert.That(jobVacancies.Items.Any(j => j.Tags.Contains("newjob_AddTeg")));
   
    }

}


