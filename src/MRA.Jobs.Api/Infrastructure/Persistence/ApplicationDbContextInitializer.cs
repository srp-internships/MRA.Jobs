using MRA.Jobs.Application.Contracts.JobVacancies;

namespace MRA.Jobs.Infrastructure.Persistence;

public class ApplicationDbContextInitializer(ApplicationDbContext dbContext)
{
    public Task SeedAsync()
    {
        return CreateNoVacancy("NoVacancy");
    }

    private async Task CreateNoVacancy(string vacancyTitle)
    {
        var noCategory =
            await dbContext.Categories.FirstOrDefaultAsync(c => c.Slug == CommonVacanciesSlugs.NoVacancySlug);
        if (noCategory == null)
        {
            var category = new VacancyCategory { Name = "NoVacancy", Slug = CommonVacanciesSlugs.NoVacancySlug };
            await dbContext.Categories.AddAsync(category);
            noCategory = category;
        }

        List<VacancyQuestion> question = new(){
            new VacancyQuestion { Question = "Your name", IsOptional = false },
            new VacancyQuestion { Question = "Your phone number", IsOptional = false }
        };

        var noVacancy = await dbContext.JobVacancies
            .Include(vacancy => vacancy.VacancyQuestions)
            .FirstOrDefaultAsync(hv => hv.Slug == CommonVacanciesSlugs.NoVacancySlug);
        if (noVacancy == null)
        {
            var vacancy = new JobVacancy
            {
                Title = vacancyTitle,
                PublishDate = DateTime.Now,
                EndDate = new DateTime(2099, 12, 31),
                ShortDescription = "",
                Description = "",
                Slug = CommonVacanciesSlugs.NoVacancySlug,
                CategoryId = noCategory.Id,
                CreatedAt = DateTime.Now,
                CreatedByEmail = "mrajobsadmin@silkroadprofessionals.com",
                VacancyQuestions = question
            };

            await dbContext.JobVacancies.AddAsync(vacancy);
        }

        if (noVacancy != null && !noVacancy.VacancyQuestions.Any())
            noVacancy.VacancyQuestions = question;

        await dbContext.SaveChangesAsync();
    }
}