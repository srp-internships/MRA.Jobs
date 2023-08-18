using MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

public class TrainingCategoriesResponce
{
    public Guid CategoryId { get; set; }
    public CategoryResponse Category { get; set; }
    public bool Selected { get; set; } = false;
}
