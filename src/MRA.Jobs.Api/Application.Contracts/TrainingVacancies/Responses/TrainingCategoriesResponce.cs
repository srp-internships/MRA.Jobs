using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
public record TrainingCategoriesResponce
{
    public Guid CategoryId { get; set; }
    public CategoryResponse Category { get; set; }
    public bool Selected { get; set; }
    public int TrainingsCount { get; set; }
}
