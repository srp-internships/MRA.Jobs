namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;

public class DeleteTrainingVacancyCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}