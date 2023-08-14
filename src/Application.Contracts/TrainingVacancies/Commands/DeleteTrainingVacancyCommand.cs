namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
public class DeleteTrainingVacancyCommand : IRequest<bool>
{
    public string Slug { get; set; }
}
