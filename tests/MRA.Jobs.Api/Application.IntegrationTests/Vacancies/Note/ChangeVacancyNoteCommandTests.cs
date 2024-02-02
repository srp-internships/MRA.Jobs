using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Vacancies.Note.Commands;
using MRA.Jobs.Application.IntegrationTests.Jobs;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Vacancies.Note;

public class ChangeVacancyNoteCommandTests : JobsContext
{
    [Test]
    public async Task ChangeVacancyNoteCommand_TestAllRoles()
    {
        ResetState();
        var vacancy = await GetJob("Vacancy with note", DateTime.Today);

        ChangeVacancyNoteCommand command = new() { VacancyId = vacancy.Id, Note = "note update" };

        RunAsReviewerAsync();
        var response = await _httpClient.PutAsJsonAsync("api/Vacancies", command);


        Assert.That((await response.Content.ReadFromJsonAsync<bool>()));
    }
}

