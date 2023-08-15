using MRA.Jobs.Domain.Entities;
namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

public class TrainingVacancyWithCategoryDto
{
    public Guid CategoryId { get; set; }
    public VacancyCategory Category { get; set; }
    public List<TrainingVacancyListDto> Trainings { get; set; }
    public int Count { get; set; }
}
