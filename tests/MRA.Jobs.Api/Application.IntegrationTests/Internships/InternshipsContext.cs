using MRA.Jobs.Application.IntegrationTests.VacancyCategories.GetCreate;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Internships;
public class InternshipsContext : Testing
{
    public async Task<InternshipVacancy> GetInternship(string title)
    {
        return await GetInternship(title, DateTime.Now.AddDays(2));
    }

    public async Task<InternshipVacancy> GetInternship(string title, DateTime endDate)
    {
        var category = new CategoryContext();
        var internship = await FindFirstOrDefaultAsync<InternshipVacancy>(i => i.Title == title);
        if (internship != null)
            return internship;

        var newInternship = new InternshipVacancy
        {
            Title = title,
            ShortDescription = "short Description",
            Description = "Description",
            PublishDate = DateTime.Now,
            EndDate =endDate,
            CategoryId = await category.GetCategoryId("Category1"),
            ApplicationDeadline = DateTime.Now.AddDays(20),
            Duration = 10,
            Stipend = 100,
            VacancyQuestions = new List<VacancyQuestion> {
                new VacancyQuestion
                {
                    Id=Guid.NewGuid(),
                    Question="Question 1?"
                },
                new VacancyQuestion
                {
                    Id = Guid.NewGuid(),
                    Question="Question 2?"
                }
            }
        };
        newInternship.Slug = newInternship.Title.ToLower().Replace(" ", "-");

        await AddAsync(newInternship);

        return newInternship;
    }
}
