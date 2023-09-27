using MRA.Jobs.Application.IntegrationTests.VacancyCategories.GetCreate;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Jobs;
public class JobsContext : Testing
{
   /// <summary>
   /// Возвращает job, если его нет то сначало создаёт в InMemory и возвращает 
   /// </summary>
   /// <param name="title"></param>
   /// <returns></returns>
    public async Task<JobVacancy> GetJob(string title)
    {
        var category = new CategoryContext();
        var Job = await FindFirstOrDefaultAsync<JobVacancy>(j => j.Title == title);
        if (Job != null)
            return Job;

        var newJob = new JobVacancy
        {
            Title = title,
            RequiredYearOfExperience = 5,
            Description = "Description",
            ShortDescription = "ShortDescription",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            CategoryId = await category.GetCategoryId("jobvacancy"),
            VacancyQuestions = new List<VacancyQuestion> {
                new VacancyQuestion{
                    Id = Guid.NewGuid(),
                    Question = "What is your English proficiency level?"} 
            },
            WorkSchedule = Domain.Enums.WorkSchedule.FullTime
        };
        newJob.Slug = newJob.Title.ToLower().Replace(" ", "-");

        await AddAsync(newJob);

        return newJob;
    }

}
