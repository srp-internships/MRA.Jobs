namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Tags;

public class AddTagToInternshipVacancyCommand : IRequest<bool>
{
    public string InternshipSlug { get; set; }
    public string[] Tags { get; set; }
}