namespace MRA.Jobs.Application.Contracts.Internships.Commands;
public class AddTagToInternshipVacancyCommand : IRequest<bool>
{
    public Guid InternshipId { get; set; }
    public Guid TagId { get; set; }
}
