namespace MRA.Jobs.Infrastructure.Persistence;

public class ApplicationDbContextInitializer(ApplicationDbContext dbContext)
{

    public async Task SeedAsync()
    {
        await CreateNoVacancy("NoVacancy");
    }
    
    private async Task CreateNoVacancy(string vacancyTitle)
    {
        var noCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == "NoVacancy");
        if (noCategory == null)
        {
            var category = new VacancyCategory() { Name = "NoVacancy", Slug = "no_vacancy" };
            await dbContext.Categories.AddAsync(category);
            noCategory = category;
        }

        var noVacancy = await dbContext.NoVacancies.FirstOrDefaultAsync(hv => hv.Title == vacancyTitle);
        if (noVacancy == null)
        {
            var vacancy = new NoVacancy()
            {
                Id = Guid.NewGuid(),
                Title = vacancyTitle,
                PublishDate = DateTime.Now,
                EndDate = new DateTime(2099, 12, 31),
                ShortDescription = "",
                Description = "",
                Slug = "no_vacancy",
                CategoryId = noCategory.Id,
                CreatedAt = DateTime.Now,
                CreatedByEmail = "mrajobsadmin@silkroadprofessionals.com"
            };

            await dbContext.NoVacancies.AddAsync(vacancy);
        }

        await dbContext.SaveChangesAsync();
    }
}