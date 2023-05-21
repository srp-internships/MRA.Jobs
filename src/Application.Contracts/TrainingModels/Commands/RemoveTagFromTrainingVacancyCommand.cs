namespace MRA.Jobs.Application.Contracts.TrainingModels.Commands;
public class RemoveTagFromTrainingVacancyCommand : IRequest<bool>
{
    public Guid TrainingModelId { get; set; }
    public Guid TagId { get; set; }
}
