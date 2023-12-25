using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Domain.Enums;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Application;
public class CreateApplicationCommandHandlerCvTests : CreateApplicationTestsBase
{
    [Ignore("i will fix it later// firuz")]
    [Test]
    public async Task CreateUsingProfile_ShouldRequestToIdentity_ReturnsOk()
    {
        await AddVacancyCategoryAsync("category111");
        var internshipSlug = await AddJobVacancyAsync("titleInternship");

        var createApplicationCommand = new CreateApplicationCommand
        {
            VacancySlug = internshipSlug, CoverLetter = RandomString(150), Cv =
            {
                IsUploadCvMode = true,
                CvBytes = new byte[]
                {
                    1, 23, 4, 4, 12, 3
                },
                FileName = "adsfasdf",
            }
        };
        RunAsDefaultUserAsync("applicant1");
        var response = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, createApplicationCommand);
        response.EnsureSuccessStatusCode();

        var application = await FindFirstOrDefaultAsync<Domain.Entities.Application>(s =>
            s.Slug == createApplicationCommand.VacancySlug && s.CoverLetter == createApplicationCommand.CoverLetter);
        application.Should().NotBeNull();
    }

    [Ignore("i will fix it later// firuz")]
    [Test]
    public async Task CreateApplicationWithNoVacancy_ShouldRequestToIdentity_ReturnsOk_IfStatusExpired()
    {
        var noVacancy = await GetNoVacancy();

        var application =
            new Domain.Entities.Application
            {
                Id = default,
                IsDeleted = false,
                CreatedAt = default,
                CreatedBy = default,
                LastModifiedAt = null,
                LastModifiedBy = default,
                CoverLetter = null,
                Slug = "1-no_vacancy",
                VacancyResponses = null,
                TaskResponses = null,
                CV = null,
                AppliedAt = DateTime.Now,
                Status = ApplicationStatus.Expired,
                ApplicantUsername = null,
                Vacancy = null,
                VacancyId = noVacancy.Id,
                ApplicantId = default,
                TestResult = null,
                History = null,
            };
        await AddAsync(application);

        var createApplicationCommand = new CreateApplicationCommand
        {
            VacancySlug = noVacancy.Slug, CoverLetter = RandomString(150),
            Cv =
            {
                IsUploadCvMode = true,
                CvBytes = new byte[]
                {
                    1, 2, 3, 4
                },
                FileName = "fasdfas",
            }
        };
        RunAsDefaultUserAsync("applicant2");
        var response = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint + "/CreateApplicationNoVacancy",
            createApplicationCommand);
        response.EnsureSuccessStatusCode();
    }
}