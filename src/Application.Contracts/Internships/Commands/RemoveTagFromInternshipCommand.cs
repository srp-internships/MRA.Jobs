using MediatR;

namespace MRA.Jobs.Application.Contracts.Internships.Commands;
public class RemoveTagFromInternshipCommand : IRequest<bool>
{
    public Guid InternshipId { get; set; }
    public string[] Tags { get; set; }
}
