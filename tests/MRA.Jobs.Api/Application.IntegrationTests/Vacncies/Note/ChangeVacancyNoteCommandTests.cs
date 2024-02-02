using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Vacancies.Note.Commands;
using MRA.Jobs.Application.IntegrationTests.Jobs;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Vacancies.Note;

public class ChangeVacancyNoteCommandTests : JobsContext
{
    [Test]
    [TestCase(UserRoles.Reviewer, true)]
    [TestCase(UserRoles.Admin, true)]
    [TestCase(UserRoles.DefaultUser, false)]
    public async Task ChangeVacancyNoteCommand_TestAllRoles(UserRoles role, bool result)
    {
        ResetState();
        var vacancy = await GetJob("Vacancy with note", DateTime.Today);

        ChangeVacancyNoteCommand command = new() { VacancyId = vacancy.Id, Note = "note update" };

        switch (role)
        {
            case UserRoles.Reviewer:
                RunAsReviewerAsync();
                break;
            case UserRoles.Admin:
                RunAsAdministratorAsync();
                break;
            case UserRoles.DefaultUser:
                RunAsDefaultUserAsync("applicant");
                break;
        }
        var response = await _httpClient.PutAsJsonAsync("api/Vacancies", command);
        
        switch (role)
        {
            case UserRoles.Reviewer:
                Assert.That((await response.Content.ReadFromJsonAsync<bool>()));
                break;
            case UserRoles.Admin:
                Assert.That((await response.Content.ReadFromJsonAsync<bool>()));
                break;
            case UserRoles.DefaultUser:
                Assert.That((await response.Content.ReadFromJsonAsync<bool>())==false);
                break;
        }
    }
}