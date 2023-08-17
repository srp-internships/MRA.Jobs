using MRA.Jobs.Domain.Entities;
namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

public class TrainingVacancyWithCategoryDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public List<TrainingVacancy> Trainings { get; set; }
    public bool Selected { get; set; } = false;
}
