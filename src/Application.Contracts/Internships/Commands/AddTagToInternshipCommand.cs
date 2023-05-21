namespace MRA.Jobs.Application.Contracts.Internships.Commands;
public class AddTagToInternshipCommand : IRequest<bool>
{
    public Guid InternshipId { get; set; }
    public Guid TagId { get; set; }
}
