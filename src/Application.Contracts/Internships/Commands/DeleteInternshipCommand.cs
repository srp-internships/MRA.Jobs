namespace MRA.Jobs.Application.Contracts.Internships.Commands;
public class DeleteInternshipCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
