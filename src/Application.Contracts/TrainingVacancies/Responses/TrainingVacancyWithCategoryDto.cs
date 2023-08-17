using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Domain.Entities;
namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

public class TrainingVacancyWithCategoryDto
{
    public Guid CategoryId { get; set; }
    public CategoryResponse Category { get; set; }
    public List<TrainingVacancyListDto> Trainings { get; set; }
    public bool Selected { get; set; } = false;
}
