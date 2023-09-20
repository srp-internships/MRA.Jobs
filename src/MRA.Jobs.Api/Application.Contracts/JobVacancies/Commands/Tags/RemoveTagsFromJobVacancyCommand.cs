namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands.Tags;

public class RemoveTagsFromJobVacancyCommand : IRequest<bool>
{
    public string JobVacancySlug { get; set; }
    public string[] Tags { get; set; }
}