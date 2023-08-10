namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
public class AddTagToInternshipVacancyCommand : IRequest<bool>
{
    public Guid InternshipId { get; set; }
    public string[] Tags { get; set; }
}
