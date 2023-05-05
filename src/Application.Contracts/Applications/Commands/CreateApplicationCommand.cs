using MediatR;

namespace MRA.Jobs.Application.Contracts.Applications.Commands;
public class CreateApplicationCommand:IRequest<Guid>
{
    public Guid ApplicantId { get; set; }
    public string CoverLetter { get; set; }
    public string History { get; set; }
    public Guid VacancyId { get; set; }
    public string ResumeUrl { get; set; }
    public int StatusId { get; set; }
}
