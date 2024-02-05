using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Vacancies.Note.Commands;
using MRA.Jobs.Application.IntegrationTests.Jobs;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Vacancies.Note;

public class ChangeVacancyNoteCommandTests : JobsContext
{
    [Test]
    [TestCase(UserRoles.Reviewer, HttpStatusCode.OK)]
    [TestCase(UserRoles.Admin, HttpStatusCode.OK)]
    [TestCase(UserRoles.DefaultUser, HttpStatusCode.Forbidden)]
    public async Task ChangeVacancyNoteCommand_TestAllRoles(UserRoles role, HttpStatusCode expectedStatusCode)
    {
        ResetState();
        var vacancy = await GetJob("Vacancy with note", DateTime.Today);

        ChangeVacancyNoteCommand command = new() { VacancyId = vacancy.Id, Note = "note update" };

        RunAsRoleAsync(role);
        var response = await _httpClient.PutAsJsonAsync("api/Vacancies/ChangeNote", command);

        Assert.That(response.StatusCode == expectedStatusCode);
    }

}