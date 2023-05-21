namespace MRA.Jobs.Application.Contracts.Applicant.Commands;
public class AddTagToApplicantCommand : IRequest<bool>
{
    public Guid ApplicantId { get; set; }
    public Guid TagId { get; set; }
}
