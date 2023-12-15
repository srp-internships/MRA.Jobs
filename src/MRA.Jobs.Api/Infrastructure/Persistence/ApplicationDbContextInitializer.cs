namespace MRA.Jobs.Infrastructure.Persistence;

public class ApplicationDbContextInitializer(ApplicationDbContext dbContext)
{

    public async Task SeedAsync()
    {
        await RemoveHiddenVacancy();
        await CreateNoVacancy("NoVacancy");
    }

    private async Task RemoveHiddenVacancy()
    {
        // Выполните SQL-запрос для удаления всех связанных записей TimelineEvents
       await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM TimelineEvents WHERE ApplicationId IN (SELECT Id FROM Applications WHERE VacancyId IN (SELECT Id FROM Vacancies WHERE Discriminator = 'HiddenVacancy'))");

        // Выполните SQL-запрос для удаления всех связанных записей Applications
       await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Applications WHERE VacancyId IN (SELECT Id FROM Vacancies WHERE Discriminator = 'HiddenVacancy')");

        // Теперь вы можете безопасно удалить запись HiddenVacancy
       await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Vacancies WHERE Discriminator = 'HiddenVacancy'");
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
            };

            await dbContext.NoVacancies.AddAsync(vacancy);
        }

        await dbContext.SaveChangesAsync();
    }
}