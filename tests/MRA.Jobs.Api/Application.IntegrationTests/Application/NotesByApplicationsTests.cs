using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Applications.Commands.AddNote;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.TimeLineDTO;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Application;

public class NotesByApplicationsTests : CreateApplicationTestsBase
{
    [Test]
    public async Task AddNoteToApplicationCommandHandler_ReturnTrue()
    {
        var vacancySlug = await AddJobVacancyAsync("vacancy");

        var createApplicationCommand = new CreateApplicationCommand
        {
            VacancySlug = vacancySlug, CoverLetter = RandomString(150), Cv = { IsUploadCvMode = false }
        };
        RunAsDefaultUserAsync("applicant1");
        var response = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, createApplicationCommand);
        response.EnsureSuccessStatusCode();

        var application =
            await FindFirstOrDefaultAsync<Domain.Entities.Application>(s =>
                s.CoverLetter == createApplicationCommand.CoverLetter);
        application.Should().NotBeNull();

        RunAsAdministratorAsync();

        AddNoteToApplicationCommand note = new() { Note = "Add note", Slug = application.Slug };

        var responseAddNoteCommand =
            await _httpClient.PostAsJsonAsync($"{ApplicationApiEndPoint}/add-note", note);
        responseAddNoteCommand.EnsureSuccessStatusCode();

        var timelineDto = (await responseAddNoteCommand.Content.ReadFromJsonAsync<TimeLineDetailsDto>());

        Assert.That(timelineDto is not null);
        Assert.That(timelineDto is { Note: "Add note" });
    }
}