namespace MRA.Jobs.Infrastructure.Persistence;

public class ApplicationDbContextInitializer(ApplicationDbContext dbContext)
{

    public async Task SeedAsync()
    {
        await CreateNoVacancy("NoVacancy");
    }

    private async Task CreateNoVacancy(string vacancyTitle)
    {
        var hiddenCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == "NoVacancy");
        if (hiddenCategory == null)
        {
            var category = new VacancyCategory() { Name = "NoVacancy", Slug = "no_vacancy" };
            await dbContext.Categories.AddAsync(category);
            hiddenCategory = category;
        }

        var hiddenVacancy = await dbContext.HiddenVacancies.FirstOrDefaultAsync(hv => hv.Title == vacancyTitle);
        if (hiddenVacancy == null)
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
                CategoryId = hiddenCategory.Id,
                CreatedAt = DateTime.Now,
            };

            await dbContext.HiddenVacancies.AddAsync(vacancy);
        }

        await dbContext.SaveChangesAsync();
    }
}