namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Tags;

public class AddTagToTrainingVacancyCommand : IRequest<bool>
{
    public string TrainingVacancySlug { get; set; }
    public string[] Tags { get; set; }
}