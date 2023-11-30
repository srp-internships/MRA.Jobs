namespace MRA.Jobs.Infrastructure.Persistence;

public class ApplicationDbContextInitializer(ApplicationDbContext dbContext)
{

    public async Task SeedAsync()
    {
        await CreateHiddenVacancy("HiddenVacancy");
    }

    private async Task CreateHiddenVacancy(string vacancyTitle)
    {
        var hiddenCategory = await dbContext.Categories.FirstAsync(c => c.Name == "Hidden Category");
        if (hiddenCategory == null)
        {
            var category = new VacancyCategory() { Name = "Hidden Category", Slug = "hidden_category" };
            await dbContext.Categories.AddAsync(category);
            hiddenCategory = category;
        }

        var hiddenVacancy = await dbContext.HiddenVacancies.FirstAsync(hv => hv.Title == vacancyTitle);
        if (hiddenVacancy == null)
        {
            var vacancy = new HiddenVacancy()
            {
                Id = Guid.NewGuid(),
                Title = vacancyTitle,
                PublishDate = DateTime.Now,
                EndDate = new DateTime(2099, 12, 31),
                ShortDescription = "",
                Description = "",
                Slug = "hidden_vacancy",
                CategoryId = hiddenCategory.Id,
                CreatedAt = DateTime.Now,
            };

            await dbContext.HiddenVacancies.AddAsync(vacancy);
        }

        await dbContext.SaveChangesAsync();
    }
}