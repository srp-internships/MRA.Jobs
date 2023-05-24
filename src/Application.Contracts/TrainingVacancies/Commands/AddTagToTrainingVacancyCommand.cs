namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
public class AddTagToTrainingVacancyCommand : IRequest<bool>
{
    public Guid VacancyId { get; set; }
    public string[] Tags { get; set; }
}