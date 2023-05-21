namespace MRA.Jobs.Application.Contracts.TrainingModels.Commands;
public class DeleteTrainingVacancyCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
