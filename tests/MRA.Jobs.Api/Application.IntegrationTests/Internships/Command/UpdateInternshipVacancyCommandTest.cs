﻿using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.IntegrationTests.VacancyCategories.GetCreate;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Internships.Command;
public class UpdateInternshipVacancyCommandTest : Testing
{

    [Test]
    public async Task UpdateInternshipVacancyCommand_UpdatingInternshipVacancyCommand_Success()
    { 
       InternshipsContext internships = new InternshipsContext();
        var internship = await internships.GetInternship("test");

        var updateCommand = new UpdateInternshipVacancyCommand
        {
            Title = "Test Internship Updated",
            ShortDescription = internship.ShortDescription,
            Description = internship.ShortDescription,
            Duration=internship.Duration,
            EndDate = internship.EndDate,
            PublishDate = internship.PublishDate,
            ApplicationDeadline = internship.ApplicationDeadline,
            CategoryId = internship.CategoryId,
            CreatedAt = internship.CreatedAt,
            Slug = internship.Slug,
            Stipend=internship.Stipend,
        };

        var response = await _httpClient.PutAsJsonAsync($"/api/internships/{internship.Slug}", updateCommand);

        response.EnsureSuccessStatusCode();
       
    }
}