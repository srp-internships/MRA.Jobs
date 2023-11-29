namespace MRA.Jobs.Infrastructure.Persistence;

public class ApplicationDbContextInitializer(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task SeedAsync()
    {
        await CreateHiddenVacancy("HiddenVacancy");
    }

    private async Task CreateHiddenVacancy(string vacancyTitle)
    {
        var vacancy = new HiddenVacancy()
        {
            Id = Guid.NewGuid(),
            Title = vacancyTitle,
            PublishDate = new DateTime(2000, 01, 01),
            EndDate = new DateTime(2099, 12, 31),
        };

        await _dbContext.HiddenVacancies.AddAsync(vacancy);
        await _dbContext.SaveChangesAsync();
    }
}