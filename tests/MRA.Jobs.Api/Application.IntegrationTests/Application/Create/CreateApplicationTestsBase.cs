using MRA.Jobs.Application.Contracts.JobVacancies;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Application;

public class CreateApplicationTestsBase : Testing
{
    private static readonly Random Random = new();
    protected const string ApplicationApiEndPoint = "/api/applications";
    
    protected async Task<Guid> AddVacancyCategoryAsync(string name)
    {
        var vacancyCategory = new VacancyCategory { Name = name, Id = Guid.NewGuid(), };
        await AddAsync(vacancyCategory);
        return vacancyCategory.Id;
    }

    protected async Task<string> AddJobVacancyAsync(string title)
    {
        var vacancy = await FindFirstOrDefaultAsync<InternshipVacancy>(v => v.Title == title);
        if (vacancy != null)
            return vacancy.Slug;

        vacancy = new InternshipVacancy
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = RandomString(50),
            ShortDescription = RandomString(10),
            Stipend = 100,
            Duration = 3,
            PublishDate = DateTime.Now,
            ApplicationDeadline = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategoryAsync("internship"),
            Slug = title.ToLower().Replace(" ", "-")
        };
        
        await AddAsync(vacancy);
        return vacancy.Slug;
    }

    protected async Task<JobVacancy> GetNoVacancy()
    {
        var vacancy = await FindFirstOrDefaultAsync<JobVacancy>(v => v.Slug == CommonVacanciesSlugs.NoVacancySlug);
        return vacancy;
    }
}