using MRA.Jobs.Application.IntegrationTests.VacancyCategories.GetCreate;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Jobs;

public class JobsContext : CategoryContext
{
    public async Task<JobVacancy> GetJob(string title, DateTime endDate)
    {
        var job = await FindFirstOrDefaultAsync<JobVacancy>(j => j.Title == title);
        if (job != null)
            return job;

        var newJob = new JobVacancy
        {
            Title = title,
            RequiredYearOfExperience = 5,
            Description = "Description",
            ShortDescription = "ShortDescription",
            PublishDate = DateTime.Now,
            EndDate = endDate,
            CategoryId = await GetCategoryId("jobvacancy"),
            VacancyQuestions =
                new List<VacancyQuestion>
                {
                    new VacancyQuestion
                    {
                        Id = Guid.NewGuid(), Question = "What is your English proficiency level?"
                    }
                },
            WorkSchedule = Domain.Enums.WorkSchedule.FullTime
        };
        newJob.Slug = newJob.Title.ToLower().Replace(" ", "-");

        await AddAsync(newJob);

        return newJob;
    }
}