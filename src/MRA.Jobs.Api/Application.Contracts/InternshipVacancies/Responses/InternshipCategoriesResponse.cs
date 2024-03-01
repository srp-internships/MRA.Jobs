using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
public record InternshipCategoriesResponse
{
    public string Slug { get; set; }
    public Guid CategoryId { get; set; }
    public CategoryResponse Category { get; set; }
    public bool Selected { get; set; }
    public int InternshipCount { get; set; }
}
