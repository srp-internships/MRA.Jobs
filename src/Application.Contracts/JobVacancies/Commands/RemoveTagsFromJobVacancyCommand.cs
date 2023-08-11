namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;

public class RemoveTagsFromJobVacancyCommand : IRequest<bool>
{
    public Guid JobVacancyId { get; set; }
    public String[] Tags { get; set; }
}