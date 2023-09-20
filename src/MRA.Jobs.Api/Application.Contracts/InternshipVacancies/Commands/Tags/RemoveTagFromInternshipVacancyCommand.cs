namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Tags;

public class RemoveTagFromInternshipVacancyCommand : IRequest<bool>
{
    public string InernshipSlug { get; set; }
    public string[] Tags { get; set; }
}