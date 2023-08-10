namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
public class RemoveTagFromTrainingVacancyCommand : IRequest<bool>
{
    public Guid VacancyId { get; set; }
    public string[] Tags { get; set; }
}
