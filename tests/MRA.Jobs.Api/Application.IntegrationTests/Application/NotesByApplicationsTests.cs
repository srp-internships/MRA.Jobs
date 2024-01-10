using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Applications.Commands.AddNote;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Application;

public class NotesByApplicationsTests : CreateApplicationTestsBase
{
    [Test]
    public async Task AddNoteToApplicationCommandHandler_ReturnTrue()
    {
        await AddVacancyCategoryAsync("category111");
        var internshipSlug = await AddJobVacancyAsync("titleInternship");

        var createApplicationCommand = new CreateApplicationCommand
        {
            VacancySlug = internshipSlug, CoverLetter = RandomString(150), Cv =
            {
                IsUploadCvMode = false
            }
        };
        RunAsDefaultUserAsync("applicant1");
        var response = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, createApplicationCommand);
        response.EnsureSuccessStatusCode();

        var application = await FindFirstOrDefaultAsync<Domain.Entities.Application>(s => s.CoverLetter == createApplicationCommand.CoverLetter);
        application.Should().NotBeNull();
        RunAsAdministratorAsync();

        AddNoteToApplicationCommand note = new() { Note = "Add note", Slug = application.Slug};

        var responseAddNoteCommand =
            await _httpClient.PostAsJsonAsync($"{ApplicationApiEndPoint}/add-note", note);
        responseAddNoteCommand.EnsureSuccessStatusCode();
        Assert.AreEqual(responseAddNoteCommand.StatusCode,HttpStatusCode.OK);
    }
}