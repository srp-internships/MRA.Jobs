using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;
using MRA.Jobs.Application.IntegrationTests.Jobs;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Vacancies.Tags;

public class RemoveTagsFromVacancyCommandTest : JobsContext
{
    private JobVacancy _vacancy;

    [SetUp]
    public async Task SetUp()
    {
        ResetState();
        _vacancy = await GetJob("Vacancy with tags", DateTime.Today);
    }

    [Test]
    public async Task DeleteTagFromVacancy()
    {
        await AddAsync(new VacancyTag() { Tag = new Tag() { Name = "tag1" }, VacancyId = _vacancy.Id });
        await AddAsync(new VacancyTag() { Tag = new Tag() { Name = "tag2" }, VacancyId = _vacancy.Id });
        await AddAsync(new VacancyTag() { Tag = new Tag() { Name = "tag3" }, VacancyId = _vacancy.Id });
        await AddAsync(new VacancyTag() { Tag = new Tag() { Name = "tag4" }, VacancyId = _vacancy.Id });

        RemoveTagsFromVacancyCommand command = new() { Tags = ["tag1", "tag4"] };
        RunAsReviewerAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/Vacancies/{_vacancy.Id}/tags", command);
        Assert.That(response.StatusCode == HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<string>>();
        Assert.That(result.Count == 2);
        Assert.That(result.Any(s => s == "tag2"));
        Assert.That(result.Any(s => s == "tag3"));
    }
}