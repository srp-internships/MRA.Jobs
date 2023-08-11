namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;

public class RemoveTagFromInternshipVacancyCommand : IRequest<bool>
{
    public Guid InternshipId { get; set; }
    public string[] Tags { get; set; }
}