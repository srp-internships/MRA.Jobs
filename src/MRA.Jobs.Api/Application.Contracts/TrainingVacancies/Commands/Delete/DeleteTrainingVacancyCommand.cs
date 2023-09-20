namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Delete;

public class DeleteTrainingVacancyCommand : IRequest<bool>
{
    public string Slug { get; set; }
}
