using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.IntegrationTests.FakeClasses;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Application;

public class CreateApplicationCommandHandlerCvTests : CreateApplicationTestsBase
{
    [Test]
    public async Task CreateUsingProfile_ShouldRequestToIdentity_ReturnsOk()
    {
        await AddVacancyCategoryAsync("category111");
        var internshipId = await AddJobVacancyAsync("titleInternship");

        var createApplicationCommand = new CreateApplicationCommand
        {
            VacancyId = internshipId,
            CoverLetter = RandomString(150),
            Cv =
            {
                IsUploadCvMode = false,
            }
        };
        RunAsDefaultUserAsync();
        var response = await _httpClient.PostAsJsonAsync("https://localhost:5001/api/applications", createApplicationCommand);
        response.EnsureSuccessStatusCode();

        var application = await FindFirstOrDefaultAsync<Domain.Entities.Application>(s => s.VacancyId == createApplicationCommand.VacancyId && s.CoverLetter == createApplicationCommand.CoverLetter);
        application.Should().NotBeNull();
    }
}