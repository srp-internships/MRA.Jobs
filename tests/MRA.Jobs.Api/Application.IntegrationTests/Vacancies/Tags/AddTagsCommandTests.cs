using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;
using MRA.Jobs.Application.IntegrationTests.Jobs;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Vacancies.Tags;

public class AddTagsCommandTests : JobsContext
{
    private JobVacancy _vacancy;

    [SetUp]
    public async Task SetUp()
    {
        _vacancy = await GetJob("Vacancy with tags", DateTime.Today);
    }

    [Test]
    public async Task AddTagsCommandTest_First()
    {
        RunAsRoleAsync(UserRoles.Reviewer);
        var response = await AddTags("tag1", "tag2", "tag3");
        response.EnsureSuccessStatusCode();
        Assert.That(response.StatusCode == HttpStatusCode.OK);
        Assert.That((await response.Content.ReadFromJsonAsync<List<string>>()).Count == 3);
    }

  
    [Test]
    public async Task AddTagsCommandTest_Second()
    {
        RunAsRoleAsync(UserRoles.Admin);
        var vacancyTag = new VacancyTag()
        {
            Tag = new Tag() { Name = "tag5" },
            VacancyId = _vacancy.Id
        };

        await AddAsync(vacancyTag);
        
        var response = await AddTags("tag7", "tag8", "tag6");
        response.EnsureSuccessStatusCode();
        Assert.That(response.StatusCode == HttpStatusCode.OK);
        Assert.That((await response.Content.ReadFromJsonAsync<List<string>>()).Count == 4);
    }
    
    private async Task<HttpResponseMessage> AddTags(params string[] tags)
    {
        var command =
            new AddTagsToVacancyCommand() { VacancyId = _vacancy.Id, Tags = tags };
        return await _httpClient.PostAsJsonAsync($"api/Vacancies/{_vacancy.Id}/tags", command);
    }

}