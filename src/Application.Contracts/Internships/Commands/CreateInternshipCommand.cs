using MediatR;

namespace MRA.Jobs.Application.Contracts.Internships.Commands;
public class CreateInternshipCommand : IRequest<Guid>
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; }
    public string RequiredSkills { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public int Duration { get; set; }
    public int Stipend { get; set; }
}
