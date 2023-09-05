using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
public record TrainingCategoriesResponse
{
    public Guid CategoryId { get; set; }
    public CategoryResponse Category { get; set; }
    public bool Selected { get; set; }
    public int TrainingsCount { get; set; }
}
