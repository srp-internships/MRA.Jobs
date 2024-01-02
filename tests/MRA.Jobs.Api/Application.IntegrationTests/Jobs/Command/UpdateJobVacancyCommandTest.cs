using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.Update;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Jobs.Command;
public class UpdateJobVacancyCommandTest : Testing
{

    [Test]
    public async Task UpdateJobVacancyCommand_UpdatingJobVacancyCommand_Success()
    {
        JobsContext jobs = new JobsContext();
        var job = await jobs.GetJob("job1", DateTime.Now.AddDays(2));

        var updateCommand = new UpdateJobVacancyCommand
        {
            Title = "job2",
            ShortDescription = job.ShortDescription,
            Description = job.ShortDescription,
            WorkSchedule = Contracts.Dtos.Enums.ApplicationStatusDto.WorkSchedule.FullTime,
            EndDate = job.EndDate,
            PublishDate = job.PublishDate,
            RequiredYearOfExperience = job.RequiredYearOfExperience,
            CategoryId = job.CategoryId,
            Slug = job.Slug,
        };

        RunAsReviewerAsync();
        var response = await _httpClient.PutAsJsonAsync($"/api/Jobs/{job.Slug}", updateCommand);

        response.EnsureSuccessStatusCode();

    }
}
