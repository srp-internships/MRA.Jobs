namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
public class RemoveTagFromTrainingVacancyCommand : IRequest<bool>
{
    public string TrainingVacancySlug { get; set; }
    public string[] Tags { get; set; }
}
