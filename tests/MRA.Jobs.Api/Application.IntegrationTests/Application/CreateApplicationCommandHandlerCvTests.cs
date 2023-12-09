using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Domain.Enums;
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
            VacancyId = internshipId, CoverLetter = RandomString(150), Cv = { IsUploadCvMode = false, }
        };
        RunAsDefaultUserAsync("applicant1");
        var response = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, createApplicationCommand);
        response.EnsureSuccessStatusCode();

        var application = await FindFirstOrDefaultAsync<Domain.Entities.Application>(s =>
            s.VacancyId == createApplicationCommand.VacancyId && s.CoverLetter == createApplicationCommand.CoverLetter);
        application.Should().NotBeNull();
    }

    [Test]
    public async Task CreateApplicationWithHiddenVacancy_1_ShouldRequestToIdentity_ReturnsOk()
    {
        var hiddenVacancy = await GetHiddenVacancy();
        var createApplicationCommand = new CreateApplicationCommand
        {
            VacancyId = hiddenVacancy.Id, CoverLetter = RandomString(150), Cv = { IsUploadCvMode = false, }
        };
        RunAsDefaultUserAsync("applicant1");
        var response = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, createApplicationCommand);
        response.EnsureSuccessStatusCode();

        var application = await FindFirstOrDefaultAsync<Domain.Entities.Application>(s =>
            s.VacancyId == createApplicationCommand.VacancyId && s.CoverLetter == createApplicationCommand.CoverLetter);
        application.Should().NotBeNull();
    }

    [Test]
    public async Task CreateApplicationWithHiddenVacancy_2_ShouldRequestToIdentity_ReturnsDuplicateException()
    {
        var hiddenVacancy = await GetHiddenVacancy();

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
                Slug = "applicant1-hidden_vacancy",
                VacancyResponses = null,
                TaskResponses = null,
                CV = null,
                AppliedAt = DateTime.Now,
                Status = ApplicationStatus.Submitted,
                ApplicantUsername = null,
                Vacancy = null,
                VacancyId = hiddenVacancy.Id,
                ApplicantId = default,
                TestResult = null,
                History = null,

            };
        await AddAsync(application);

        var createApplicationCommand = new CreateApplicationCommand
        {
            VacancyId = hiddenVacancy.Id, CoverLetter = RandomString(150), Cv = { IsUploadCvMode = false, }
        };
        RunAsDefaultUserAsync("applicant1");
        var response = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, createApplicationCommand);
        var exception = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(response.StatusCode, HttpStatusCode.Conflict);
        Assert.IsTrue(exception.Contains("Duplicate"));
    }
    
    [Test]
    public async Task CreateApplicationWithHiddenVacancy_3_ShouldRequestToIdentity_ReturnsOk_IfStatusExpired()
    {
        var hiddenVacancy = await GetHiddenVacancy();
        
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
                Slug = "applicant2-hidden_vacancy",
                VacancyResponses = null,
                TaskResponses = null,
                CV = null,
                AppliedAt = DateTime.Now,
                Status = ApplicationStatus.Expired,
                ApplicantUsername = null,
                Vacancy = null,
                VacancyId = hiddenVacancy.Id,
                ApplicantId = default,
                TestResult = null,
                History = null,

            };
        await AddAsync(application);

        var createApplicationCommand = new CreateApplicationCommand
        {
            VacancyId = hiddenVacancy.Id, CoverLetter = RandomString(150), Cv = { IsUploadCvMode = false, }
        };
        RunAsDefaultUserAsync("applicant2");
        var response = await _httpClient.PostAsJsonAsync(ApplicationApiEndPoint, createApplicationCommand);
        response.EnsureSuccessStatusCode();
    }
    
}