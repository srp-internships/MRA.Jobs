using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
public record InternshipCategoriesResponce
{
    public Guid CategoryId { get; set; }
    public CategoryResponse Category { get; set; }
    public bool Selected { get; set; }
    public int InternshipCount { get; set; }
}
