namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;

public class RemoveTagsFromJobVacancyCommand : IRequest<bool>
{
    public string JobVacancySlug { get; set; }
    public String[] Tags { get; set; }
}


